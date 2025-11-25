using System;
using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Hospital.domain.model;
using Hospital.domain.ports;
using Hospital.infraestructure;

namespace Hospital.infraestructure.adapters.output
{
    public class SQLMedicalInsurancePort : Medical_insurance_port
    {
        private SqlConnection GetConn() => new SqlConnection(Config.SqlConnectionString);

        public Medical_insurance FindByIdSure(Medical_insurance insurance)
        {
            if (insurance == null) return null!;
            const string sql = @"
SELECT
    MedicalInsuranceId AS IdSure,
    CompanyName      AS Company_name,
    PolicyNumber     AS Policy_number,  
    PolicyStatus     AS Policy_status,
    EffectiveDate    AS Effective_Date,
    Copayment        AS Copayment,
    InsuranceCost    AS Insurance_cost
FROM dbo.MedicalInsurance
WHERE MedicalInsuranceId = @id";

            using var cn = GetConn();
            cn.Open();

            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = insurance.IdSure;
            Debug.WriteLine($"SQLMedicalInsurancePort.FindByIdSure: sql={sql} @id={insurance.IdSure}");

            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null!;

            return new Medical_insurance
            {
                IdSure = rd["IdSure"] != DBNull.Value ? Convert.ToInt32(rd["IdSure"]) : 0,
                Company_name = rd["Company_name"]?.ToString() ?? string.Empty,
                Policy_number = rd["Policy_number"]?.ToString() ?? string.Empty,
                Policy_status = rd["Policy_status"] != DBNull.Value && Convert.ToBoolean(rd["Policy_status"]),
                Effective_Date = rd["Effective_Date"] != DBNull.Value ? Convert.ToDateTime(rd["Effective_Date"]) : default,
                Copayment = rd["Copayment"] != DBNull.Value ? Convert.ToInt64(rd["Copayment"]) : 0,
                Insurance_cost = rd["Insurance_cost"] != DBNull.Value ? Convert.ToInt64(rd["Insurance_cost"]) : 0
            };
        }

        public Medical_insurance FindByPolicy_number(Medical_insurance insurance)
        {
            if (insurance == null) return null!;
            const string sql = @"
SELECT TOP(1)
    MedicalInsuranceId AS IdSure,
    CompanyName      AS Company_name,
    PolicyNumber     AS Policy_number,
    PolicyStatus     AS Policy_status,
    EffectiveDate    AS Effective_Date,
    Copayment        AS Copayment,
    InsuranceCost    AS Insurance_cost
FROM dbo.MedicalInsurance
WHERE PolicyNumber = @policy";

            using var cn = GetConn();
            cn.Open();

            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@policy", SqlDbType.NVarChar, 200).Value = insurance.Policy_number ?? string.Empty;
            Debug.WriteLine($"SQLMedicalInsurancePort.FindByPolicy_number: sql={sql} @policy={insurance.Policy_number}");

            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null!;

            return new Medical_insurance
            {
                IdSure = rd["IdSure"] != DBNull.Value ? Convert.ToInt32(rd["IdSure"]) : 0,
                Company_name = rd["Company_name"]?.ToString() ?? string.Empty,
                Policy_number = rd["Policy_number"]?.ToString() ?? string.Empty,
                Policy_status = rd["Policy_status"] != DBNull.Value && Convert.ToBoolean(rd["Policy_status"]),
                Effective_Date = rd["Effective_Date"] != DBNull.Value ? Convert.ToDateTime(rd["Effective_Date"]) : default,
                Copayment = rd["Copayment"] != DBNull.Value ? Convert.ToInt64(rd["Copayment"]) : 0,
                Insurance_cost = rd["Insurance_cost"] != DBNull.Value ? Convert.ToInt64(rd["Insurance_cost"]) : 0
            };
        }

        public void Save(Medical_insurance insurance)
        {
            const string insert = @"
INSERT INTO dbo.MedicalInsurance (CompanyName, PolicyNumber, PolicyStatus, EffectiveDate, Copayment, InsuranceCost)
VALUES (@company, @policyNumber, @status, @effectiveDate, @copayment, @insuranceCost);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var cn = GetConn();
            cn.Open();
            using var tx = cn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(insert, cn, tx);
                cmd.Parameters.AddWithValue("@company", !string.IsNullOrWhiteSpace(insurance.Company_name) ? (object)insurance.Company_name : DBNull.Value);
                cmd.Parameters.AddWithValue("@policyNumber", !string.IsNullOrWhiteSpace(insurance.Policy_number) ? (object)insurance.Policy_number : DBNull.Value);
                cmd.Parameters.AddWithValue("@status", insurance.Policy_status);
                cmd.Parameters.AddWithValue("@effectiveDate", insurance.Effective_Date == default ? (object)DBNull.Value : insurance.Effective_Date);
                cmd.Parameters.AddWithValue("@copayment", insurance.Copayment != 0 ? (object)insurance.Copayment : DBNull.Value);
                cmd.Parameters.AddWithValue("@insuranceCost", insurance.Insurance_cost != 0 ? (object)insurance.Insurance_cost : DBNull.Value);

                var idObj = cmd.ExecuteScalar();
                insurance.IdSure = idObj != null && idObj != DBNull.Value ? Convert.ToInt32(idObj) : 0;
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public void Update(Medical_insurance insurance)
        {
            if (insurance == null) throw new ArgumentNullException(nameof(insurance));
            if (insurance.IdSure == 0) throw new ArgumentException("IdSure es obligatorio para actualizar.");

            const string update = @"
UPDATE dbo.MedicalInsurance
SET CompanyName = @company,
    PolicyNumber = @policyNumber,
    PolicyStatus = @status,
    EffectiveDate = @effectiveDate,
    Copayment = @copayment,
    InsuranceCost = @insuranceCost
WHERE MedicalInsuranceId = @id;";

            using var cn = GetConn();
            cn.Open();
            using var tx = cn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(update, cn, tx);
                cmd.Parameters.AddWithValue("@company", !string.IsNullOrWhiteSpace(insurance.Company_name) ? (object)insurance.Company_name : DBNull.Value);
                cmd.Parameters.AddWithValue("@policyNumber", !string.IsNullOrWhiteSpace(insurance.Policy_number) ? (object)insurance.Policy_number : DBNull.Value);
                cmd.Parameters.AddWithValue("@status", insurance.Policy_status);
                cmd.Parameters.AddWithValue("@effectiveDate", insurance.Effective_Date == default ? (object)DBNull.Value : insurance.Effective_Date);
                cmd.Parameters.AddWithValue("@copayment", insurance.Copayment != 0 ? (object)insurance.Copayment : DBNull.Value);
                cmd.Parameters.AddWithValue("@insuranceCost", insurance.Insurance_cost != 0 ? (object)insurance.Insurance_cost : DBNull.Value);
                cmd.Parameters.AddWithValue("@id", insurance.IdSure);
                cmd.ExecuteNonQuery();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
    }
}