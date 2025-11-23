using Hospital.domain.model;
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Hospital.infraestructure.adapters
{
    public class SqlPatientPort
    {
        private SqlConnection GetConn() => new SqlConnection(Config.SqlConnectionString);

        // ================================
        // FIND BY DOCUMENT = Id_patient
        // ================================
        public Patient FindByDocument(Patient patient)
        {
            if (patient == null || string.IsNullOrWhiteSpace(patient.Id_patient))
                return null;

            const string sql = @"
                SELECT TOP(1)
                    patient_id,
                    gender,
                    contact,
                    direction,
                    name,
                    birth,
                    email,
                    cellphone
                FROM dbo.patients
                WHERE patient_id = @doc";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@doc", patient.Id_patient);

            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);

            return rd.Read() ? Map(rd) : null;
        }


        // ================================
        // FIND BY ID
        // ================================
        public Patient FindById(string patientId)
        {
            if (string.IsNullOrWhiteSpace(patientId))
                return null;

            const string sql = @"
                SELECT TOP(1)
                    patient_id,
                    gender,
                    contact,
                    direction,
                    name,
                    birth,
                    email,
                    cellphone
                FROM dbo.patients
                WHERE patient_id = @id";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@id", patientId);

            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);

            return rd.Read() ? Map(rd) : null;
        }


        // ================================
        // EXISTS
        // ================================
        public bool PatientExists(string patientId)
        {
            const string sql = @"SELECT TOP(1) 1 FROM dbo.patients WHERE patient_id = @id";

            using var cn = GetConn();
            cn.Open();

            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", patientId);

            return cmd.ExecuteScalar() != null;
        }


        // ================================
        // SAVE
        // ================================
        public void Save(Patient patient)
        {
            using var cn = GetConn();
            cn.Open();

            const string insert = @"
                INSERT INTO dbo.patients
                    (patient_id, gender, contact, direction, name, birth, email, cellphone)
                VALUES
                    (@id, @gender, @contact, @direction, @name, @birth, @email, @cellphone);";

            using var cmd = new SqlCommand(insert, cn);

            cmd.Parameters.AddWithValue("@id", patient.Id_patient);
            cmd.Parameters.AddWithValue("@gender", patient.Gender ? 1 : 0);
            cmd.Parameters.AddWithValue("@contact", patient.Contact?.Relation ?? "");
            cmd.Parameters.AddWithValue("@direction", patient.Direction1?.Direction ?? patient.Direction ?? "");
            cmd.Parameters.AddWithValue("@name", patient.Name1?.Name ?? patient.Name ?? "");
            cmd.Parameters.AddWithValue("@birth", patient.Birth1?.Birth ?? patient.Birth);
            cmd.Parameters.AddWithValue("@email", patient.Email1?.Email?.Address ?? patient.Email?.Address ?? "");
            cmd.Parameters.AddWithValue("@cellphone", patient.Cellphone1?.Cellphone ?? patient.Cellphone);

            cmd.ExecuteNonQuery();
        }


        // ================================
        // UPDATE
        // ================================
        public void Update(Patient p)
        {
            const string sql = @"
                UPDATE dbo.patients
                SET 
                    gender = @gender,
                    contact = @contact,
                    direction = @direction,
                    name = @name,
                    birth = @birth,
                    email = @email,
                    cellphone = @cellphone
                WHERE patient_id = @id";

            using var cn = GetConn();
            cn.Open();

            using var cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@id", p.Id_patient);
            cmd.Parameters.AddWithValue("@gender", p.Gender ? 1 : 0);
            cmd.Parameters.AddWithValue("@contact", p.Contact?.Relation ?? "");
            cmd.Parameters.AddWithValue("@direction", p.Direction1?.Direction ?? p.Direction ?? "");
            cmd.Parameters.AddWithValue("@name", p.Name1?.Name ?? p.Name ?? "");
            cmd.Parameters.AddWithValue("@birth", p.Birth1?.Birth ?? p.Birth);
            cmd.Parameters.AddWithValue("@email", p.Email1?.Email?.Address ?? p.Email?.Address ?? "");
            cmd.Parameters.AddWithValue("@cellphone", p.Cellphone1?.Cellphone ?? p.Cellphone);

            cmd.ExecuteNonQuery();
        }


        // ================================
        // MAP
        // ================================
        private static Patient Map(SqlDataReader rd)
        {
            return new Patient
            {
                Id_patient = rd["patient_id"]?.ToString(),
                Gender = Convert.ToBoolean(rd["gender"]),
                Direction1 = new Person { Direction = rd["direction"]?.ToString() },
                Name1 = new Person { Name = rd["name"]?.ToString() },
                Birth1 = new Person { Birth = Convert.ToDateTime(rd["birth"]) },
                Email1 = new Person { Email = new System.Net.Mail.MailAddress(rd["email"]?.ToString() ?? "") },
                Cellphone1 = new Person { Cellphone = Convert.ToInt64(rd["cellphone"]) },
                Contact = new Contact { Relation = rd["contact"]?.ToString() }
            };
        }
    }
}
