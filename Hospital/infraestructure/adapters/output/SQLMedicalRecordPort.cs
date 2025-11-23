using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Hospital.domain.model;

namespace Hospital.infraestructure.adapters.output
{
    public class SQLMedicalRecordPort
    {
        private SqlConnection GetConn() => new SqlConnection(Config.SqlConnectionString);

        public object FindByPatientId(string patientId)
        {
            const string sql = @"
                SELECT v.VisitId, v.PatientId, v.DoctorUserId, v.OrderProcedureId, v.VitalDataId, v.VisitDate
                FROM dbo.Visit v
                WHERE v.PatientId = @patientId
                ORDER BY v.VisitDate DESC;";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@patientId", patientId ?? string.Empty);
            using var rd = cmd.ExecuteReader();
            if (!rd.HasRows) return null;

            // Retornar primer registro como ejemplo
            if (rd.Read())
            {
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

            return null;
        }
    }
}