using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Hospital.infraestructure.adapters.output
{
    public class SQLBillingPort : Billings_port
    {
        private SqlConnection GetConn() => new SqlConnection(Config.SqlConnectionString);

        public Billings FindById_billings(Billings billings)
        {
            if (billings == null) return null;
            const string sql = @"
                SELECT b.BillingId, b.PatientId, b.OrderId, b.MedicalInsuranceId, b.Amount, b.InsuranceCost, b.CreatedAt
                FROM dbo.Billings b
                WHERE b.BillingId = @billingId OR b.PatientId = @patientId";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@billingId", billings.Id_billings);
            cmd.Parameters.AddWithValue("@patientId", billings.Id_patient ?? string.Empty);
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            var res = new Billings();
            res.Id_billings = rd["BillingId"] != DBNull.Value ? Convert.ToInt32(rd["BillingId"]) : 0;
            res.Id_patient = rd["PatientId"]?.ToString();
            res.Amount = rd["Amount"] != DBNull.Value ? Convert.ToInt64(rd["Amount"]) : 0;
            return res;
        }

        public Order FindByNumOrder(Billings billings)
        {
            if (billings == null) return null;
            if (billings.Id_billings == 0 && string.IsNullOrEmpty(billings.Id_patient)) return null;

            const string sql = @"
                SELECT o.OrderId
                FROM dbo.Billings b
                INNER JOIN dbo.Orders o ON b.OrderId = o.OrderId
                WHERE b.BillingId = @billingId OR b.PatientId = @patientId";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@billingId", billings.Id_billings);
            cmd.Parameters.AddWithValue("@patientId", billings.Id_patient ?? string.Empty);
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            return new Order { IdOrder1 = rd["OrderId"] != DBNull.Value ? Convert.ToInt32(rd["OrderId"]) : 0 };
        }

        public void Save(Billings billings)
        {
            const string sql = @"
                INSERT INTO dbo.Billings (PatientId, OrderId, MedicalInsuranceId, Amount, InsuranceCost)
                VALUES (@patientId, @orderId, @medicalInsuranceId, @amount, @insuranceCost);";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@patientId", billings.Id_patient ?? string.Empty);
            cmd.Parameters.AddWithValue("@orderId", billings.Order != null ? (object) (billings.Order.IdOrder1) : DBNull.Value);
            cmd.Parameters.AddWithValue("@medicalInsuranceId", billings.Company_name != null ? (object) billings.Company_name.IdSure : DBNull.Value);
            cmd.Parameters.AddWithValue("@amount", billings.Amount);
            cmd.Parameters.AddWithValue("@insuranceCost", billings.Insurance_cost != null ? (object) billings.Insurance_cost.Insurance_cost : DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public void Update(Billings billings)
        {
            const string sql = @"
                UPDATE dbo.Billings
                SET PatientId = @patientId, OrderId = @orderId, MedicalInsuranceId = @medicalInsuranceId, Amount = @amount, InsuranceCost = @insuranceCost
                WHERE BillingId = @billingId;";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@patientId", billings.Id_patient ?? string.Empty);
            cmd.Parameters.AddWithValue("@orderId", billings.Order != null ? (object) (billings.Order.IdOrder1) : DBNull.Value);
            cmd.Parameters.AddWithValue("@medicalInsuranceId", billings.Company_name != null ? (object) billings.Company_name.IdSure : DBNull.Value);
            cmd.Parameters.AddWithValue("@amount", billings.Amount);
            cmd.Parameters.AddWithValue("@insuranceCost", billings.Insurance_cost != null ? (object) billings.Insurance_cost.Insurance_cost : DBNull.Value);
            cmd.Parameters.AddWithValue("@billingId", billings.Id_billings);
            cmd.ExecuteNonQuery();
        }
    }
}
