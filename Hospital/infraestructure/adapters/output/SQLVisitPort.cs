using Hospital.domain.model;
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Hospital.infraestructure.adapters.output
{
    public class SqlVisitPort
    {
        private SqlConnection GetConn()
            => new SqlConnection(Config.SqlConnectionString);

        // ============================================================
        // FIND BY ID
        // ============================================================
        public Visit FindById(long visitId)
        {
            const string sql = @"
                SELECT TOP (1)
                    VisitId,
                    PatientId,
                    DoctorUserId,
                    OrderProcedureId,
                    VitalDataId
                FROM dbo.Visit
                WHERE VisitId = @id";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", visitId);

            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            return rd.Read() ? MapVisit(rd) : null;
        }

        // ============================================================
        // SAVE
        // ============================================================
        public void Save(Visit visit)
        {
            const string sql = @"
                INSERT INTO dbo.Visit 
                    (PatientId, DoctorUserId, OrderProcedureId, VitalDataId)
                VALUES 
                    (@patientId, @doctorId, @procedureId, @vitalDataId);";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@patientId", visit.Id_patient?.Id_patient ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@doctorId", visit.Id_doctor?.Id_doctor ?? (object)DBNull.Value);

            // CAMBIO IMPORTANTE: tu modelo no tiene OrderProcedureId
            cmd.Parameters.AddWithValue("@procedureId",
                visit.Id_procedure?.IdProcedure ?? (object)DBNull.Value);

            // CAMBIO IMPORTANTE: tu modelo no tiene VitalDataId, sino Id_vitalData
            cmd.Parameters.AddWithValue("@vitalDataId",
                visit.Vital_data ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        // ============================================================
        // MAP
        // ============================================================
        private static Visit MapVisit(SqlDataReader rd)
        {
            var visit = new Visit();

            // Patient
            visit.Id_patient = new Patient
            {
                Id_patient = rd["PatientId"]?.ToString()
            };

            // Doctor
            visit.Id_doctor = new Doctor
            {
                Id_doctor = rd["DoctorUserId"]?.ToString()
            };

            // Order Procedure
            visit.Id_procedure = new Order_procedure
            {
                IdProcedure = rd["OrderProcedureId"] != DBNull.Value
                    ? Convert.ToInt64(rd["OrderProcedureId"])
                    : 0
            };

            // Vital Data
            visit.Vital_data = new Vital_data
            {
                // No existe Id_vitalData, así que no se debe asignar aquí.
                // Puedes eliminar esta línea o asignar las propiedades reales si es necesario.
            };

            return visit;
        }
    }
}
