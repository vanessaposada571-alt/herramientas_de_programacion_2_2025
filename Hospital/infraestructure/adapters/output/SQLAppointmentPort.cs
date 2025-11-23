using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Hospital.infraestructure.adapters.output
{
    public class SQLAppointmentPort
    {
        private SqlConnection GetConn() => new SqlConnection(Config.SqlConnectionString);

        // Implementación de ejemplo: obtener visita/orden por Id
        public object FindById(object probe)
        {
            const string sql = @"
                SELECT v.VisitId, v.PatientId, v.DoctorUserId, v.OrderProcedureId, v.VitalDataId, v.VisitDate
                FROM dbo.Visit v
                WHERE v.VisitId = @visitId;";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@visitId", probe ?? DBNull.Value);
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            return new
            {
                VisitId = rd["VisitId"],
                PatientId = rd["PatientId"],
                DoctorUserId = rd["DoctorUserId"],
                OrderProcedureId = rd["OrderProcedureId"],
                VitalDataId = rd["VitalDataId"],
                VisitDate = rd["VisitDate"]
            };
        }
    }
}