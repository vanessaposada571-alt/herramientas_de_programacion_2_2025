using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class visita
    {
        private paciente paciente;
        private medico medico;
        private orden_procedimiento procedimiento;
        private datos_vitales datos_vitales;
        private String observaciones;

        public string Observaciones { get => observaciones; set => observaciones = value; }
        internal paciente Paciente { get => paciente; set => paciente = value; }
        internal medico Medico { get => medico; set => medico = value; }
        internal orden_procedimiento Procedimiento { get => procedimiento; set => procedimiento = value; }
        internal datos_vitales Datos_vitales { get => datos_vitales; set => datos_vitales = value; }
    }
}
