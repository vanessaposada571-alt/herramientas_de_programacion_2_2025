using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
    
namespace Hospital.infraestructure.adapters.output
{
    public class SqlOrderPort
    {
        private SqlConnection GetConn() => new SqlConnection(Config.SqlConnectionString);

        public Order FindByPatient(string patientId)
        {
            const string sql = @"
                SELECT TOP (1) order_number, patient_document, doctor_document, creation_date
                FROM dbo.orders
                WHERE patient_document = @patientDoc";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@patientDoc", patientId ?? "");

            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            return rd.Read() ? MapOrder(rd) : null;
        }

        // Mantengo el viejo método pero lo marco como no soportado para evitar usos incorrectos.
        public void Save(Order order)
        {
            throw new InvalidOperationException(
                "Save(Order) no puede deducir el documento de paciente/doctor. " +
                "Use Save(Order, Patient, Doctor, DateTime creationDate) proporcionando los objetos existentes."
            );
        }

        // Nueva sobrecarga: recibe los objetos Patient y Doctor ya existentes en el dominio.
        public void Save(Order order, Patient patient, Doctor doctor, DateTime creationDate)
        {
            const string sql = @"
                INSERT INTO dbo.orders (order_number, patient_document, doctor_document, creation_date)
                VALUES (@orderNum, @patientDoc, @doctorDoc, @creationDate)";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@orderNum", order.IdOrder1);

            // Uso las propiedades existentes en Patient y Doctor sin modificar Order
            var patientDocValue = patient?.Id_patient ?? string.Empty;
            var doctorDocValue = doctor?.Id_doctor ?? string.Empty;

            cmd.Parameters.AddWithValue("@patientDoc", patientDocValue);
            cmd.Parameters.AddWithValue("@doctorDoc", doctorDocValue);
            cmd.Parameters.AddWithValue("@creationDate", creationDate);

            cmd.ExecuteNonQuery();
        }

        private static Order MapOrder(SqlDataReader rd)
        {
            // Order no contiene campos para patient/doctor/creation en el modelo actual,
            // así que mapeamos únicamente lo que existe en Order.
            return new Order
            {
                IdOrder1 = (int)rd["order_number"]
                // Si en el futuro Order expone campos para documento/fecha, añadirlos aquí.
            };
        }
    }
}