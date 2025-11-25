using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Net.Mail;

namespace Hospital.infraestructure.adapters.output
{
    public class SQLPatientPort : Patient_port
    {
        private SqlConnection GetConn() => new SqlConnection(Config.SqlConnectionString);

        public Patient FindById_patient(Patient patient)
        {
            if (patient == null) return null;
            const string sql = @"
                SELECT TOP(1)
                       p.PatientId, p.PolicyNumber,
                       per.PersonId, per.Name, per.IdNumber, per.Email, per.Cellphone, per.BirthDate, per.Gender, per.Direction,
                       mi.MedicalInsuranceId, mi.CompanyName, mi.PolicyNumber AS MiPolicyNumber, mi.EffectiveDate,
                       c.ContactId, c.PersonId AS ContactPersonId, c.Name AS ContactName, c.Cellphone AS ContactCellphone, c.Relation AS ContactRelation
                FROM dbo.Patient p
                LEFT JOIN dbo.Person per ON p.PersonId = per.PersonId
                LEFT JOIN dbo.MedicalInsurance mi ON p.MedicalInsuranceId = mi.MedicalInsuranceId
                LEFT JOIN dbo.Contact c ON c.PatientId = p.PatientId
                WHERE p.PatientId = @patientId";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@patientId", patient.Id_patient ?? string.Empty);
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            var res = new Patient();
            res.Id_patient = rd["PatientId"]?.ToString();

            var name = rd["Name"]?.ToString();
            if (!string.IsNullOrWhiteSpace(name)) res.Name1 = new Person { Name = name };
            if (rd["Gender"] != DBNull.Value) res.Gender1 = new Person { Gender = Convert.ToBoolean(rd["Gender"]) };

            var emailStr = rd["Email"]?.ToString();
            if (!string.IsNullOrWhiteSpace(emailStr))
                res.Email1 = new Person { Email = new MailAddress(emailStr) };

            // Mapear seguro
            if (rd["MedicalInsuranceId"] != DBNull.Value)
            {
                res.IdSure = new Medical_insurance { IdSure = Convert.ToInt32(rd["MedicalInsuranceId"]) };
                res.PolicyNumber = rd["MiPolicyNumber"]?.ToString();
            }

            // Mapear contacto de emergencia (si existe)
            if (rd["ContactId"] != DBNull.Value)
            {
                var contact = new Contact();
                var contactName = rd["ContactName"]?.ToString();
                if (!string.IsNullOrWhiteSpace(contactName))
                    contact.Name1 = new Person { Name = contactName };

                if (rd["ContactCellphone"] != DBNull.Value)
                    contact.Cellphone = new Person { Cellphone = Convert.ToInt64(rd["ContactCellphone"]) };

                contact.Relation = rd["ContactRelation"]?.ToString() ?? string.Empty;

                if (rd["ContactPersonId"] != DBNull.Value)
                    contact.PersonId = Convert.ToInt64(rd["ContactPersonId"]);

                res.Contact = contact;
            }

            return res;
        }

        public Patient FindByContact(Patient patient)
        {
            const string sql = @"
                SELECT p.PatientId, per.Name, c.Name AS ContactName, c.Cellphone, c.Relation
                FROM dbo.Patient p
                LEFT JOIN dbo.Person per ON p.PersonId = per.PersonId
                LEFT JOIN dbo.Contact c ON c.PatientId = p.PatientId
                WHERE c.Cellphone = @cellphone";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@cellphone", patient.Cellphone1?.Cellphone ?? 0L);
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            return rd.Read() ? new Patient { Id_patient = rd["PatientId"]?.ToString() ?? "" } : null;
        }

        public Patient FindByIdSure(Patient patient)
        {
            const string sql = @"
                SELECT p.PatientId
                FROM dbo.Patient p
                WHERE p.MedicalInsuranceId = @miId";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@miId", patient.IdSure?.IdSure ?? (object)DBNull.Value);
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            return rd.Read() ? new Patient { Id_patient = rd["PatientId"]?.ToString() ?? "" } : null;
        }

        public void Save(Patient patient)
        {
            const string insertPerson = @"
                INSERT INTO dbo.Person (Name, IdNumber, Email, Cellphone, BirthDate, Gender, Direction)
                VALUES (@name, @idNumber, @email, @cellphone, @birthDate, @gender, @direction);
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

            const string insertPatient = @"
                INSERT INTO dbo.Patient (PatientId, PersonId, MedicalInsuranceId, PolicyNumber)
                VALUES (@patientId, @personId, @medicalInsuranceId, @policyNumber);";

            const string insertContact = @"
                INSERT INTO dbo.Contact (PatientId, PersonId, Name, Cellphone, Relation)
                VALUES (@patientId, @personId, @contactName, @contactCellphone, @contactRelation);";

            using var cn = GetConn();
            cn.Open();
            using var tx = cn.BeginTransaction();
            try
            {
                using var cmd1 = new SqlCommand(insertPerson, cn, tx);
                cmd1.Parameters.AddWithValue("@name", patient.Name1?.Name ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@idNumber", patient.Name1?.Id ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@email", !string.IsNullOrWhiteSpace(patient.Email1?.Email?.Address) ? (object)patient.Email1.Email.Address : DBNull.Value);
                cmd1.Parameters.AddWithValue("@cellphone", patient.Cellphone1?.Cellphone ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@birthDate", (object)(patient.Birth == default ? DBNull.Value : patient.Birth));
                cmd1.Parameters.AddWithValue("@gender", patient.Gender1?.Gender == true ? 1 : (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@direction", !string.IsNullOrWhiteSpace(patient.Direction1?.Direction) ? (object)patient.Direction1.Direction : DBNull.Value);
                var personIdObj = cmd1.ExecuteScalar();
                if (personIdObj == null || personIdObj == DBNull.Value) throw new Exception("No se obtuvo PersonId al insertar Person.");
                var personId = (long)personIdObj;

                using var cmd2 = new SqlCommand(insertPatient, cn, tx);
                cmd2.Parameters.AddWithValue("@patientId", patient.Id_patient ?? Guid.NewGuid().ToString());
                cmd2.Parameters.AddWithValue("@personId", personId);
                cmd2.Parameters.AddWithValue("@medicalInsuranceId", patient.IdSure?.IdSure ?? (object)DBNull.Value);
                cmd2.Parameters.AddWithValue("@policyNumber", patient.PolicyNumber ?? (object)DBNull.Value);
                cmd2.ExecuteNonQuery();

                // Insertar contacto de emergencia si se proporcionó
                if (patient.Contact != null)
                {
                    using var cmd3 = new SqlCommand(insertContact, cn, tx);
                    cmd3.Parameters.AddWithValue("@patientId", patient.Id_patient ?? Guid.NewGuid().ToString());
                    cmd3.Parameters.AddWithValue("@personId", personId);
                    var cName = patient.Contact.Name1?.Name ?? patient.Contact.Name ?? string.Empty;
                    cmd3.Parameters.AddWithValue("@contactName", !string.IsNullOrWhiteSpace(cName) ? (object)cName : DBNull.Value);
                    cmd3.Parameters.AddWithValue("@contactCellphone", patient.Contact.Cellphone?.Cellphone ?? (object)DBNull.Value);
                    cmd3.Parameters.AddWithValue("@contactRelation", !string.IsNullOrWhiteSpace(patient.Contact.Relation) ? (object)patient.Contact.Relation : DBNull.Value);
                    cmd3.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public void Update(Patient p)
        {
            const string updatePerson = @"
                UPDATE dbo.Person
                SET Name = @name, Email = @email, Cellphone = @cellphone, BirthDate = @birthDate, Gender = @gender, Direction = @direction
                WHERE PersonId = (SELECT PersonId FROM dbo.Patient WHERE PatientId = @patientId);";

            const string updateContact = @"
                UPDATE dbo.Contact
                SET PersonId = @PersonId, Name = @contactName, Cellphone = @contactCellphone, Relation = @contactRelation
                WHERE PatientId = @patientId;";

            const string insertContact = @"
                INSERT INTO dbo.Contact (PatientId, PersonId, Name, Cellphone, Relation)
                VALUES (@patientId, @PersonId, @contactName, @contactCellphone, @contactRelation);";

            using var cn = GetConn();
            cn.Open();
            using var tx = cn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(updatePerson, cn, tx);
                cmd.Parameters.AddWithValue("@name", p.Name1?.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@email", p.Email1?.Email?.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@cellphone", p.Cellphone1?.Cellphone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@birthDate", (object)(p.Birth == default ? DBNull.Value : p.Birth));
                cmd.Parameters.AddWithValue("@gender", p.Gender1?.Gender == true ? 1 : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@direction", p.Direction1?.Direction ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@patientId", p.Id_patient ?? string.Empty);
                cmd.ExecuteNonQuery();

                // Obtener PersonId asociado a este Patient
                long PersonId = 0;
                const string selectPersonId = "SELECT PersonId FROM dbo.Patient WHERE PatientId = @patientId";
                using (var cmdSelect = new SqlCommand(selectPersonId, cn, tx))
                {
                    cmdSelect.Parameters.AddWithValue("@patientId", p.Id_patient ?? string.Empty);
                    var pidObj = cmdSelect.ExecuteScalar();
                    if (pidObj != null && pidObj != DBNull.Value) PersonId = Convert.ToInt64(pidObj);
                }

                // Actualizar contacto; si no existe, insertar
                using var cmdContact = new SqlCommand(updateContact, cn, tx);
                var cName = p.Contact?.Name1?.Name ?? p.Contact?.Name ?? string.Empty;
                cmdContact.Parameters.AddWithValue("@PersonId", PersonId != 0 ? (object)PersonId : DBNull.Value);
                cmdContact.Parameters.AddWithValue("@contactName", !string.IsNullOrWhiteSpace(cName) ? (object)cName : DBNull.Value);
                cmdContact.Parameters.AddWithValue("@contactCellphone", p.Contact?.Cellphone?.Cellphone ?? (object)DBNull.Value);
                cmdContact.Parameters.AddWithValue("@contactRelation", !string.IsNullOrWhiteSpace(p.Contact?.Relation) ? (object)p.Contact.Relation : DBNull.Value);
                cmdContact.Parameters.AddWithValue("@patientId", p.Id_patient ?? string.Empty);
                var rows = cmdContact.ExecuteNonQuery();

                if (rows == 0 && p.Contact != null)
                {
                    using var cmdInsertContact = new SqlCommand(insertContact, cn, tx);
                    cmdInsertContact.Parameters.AddWithValue("@patientId", p.Id_patient ?? string.Empty);
                    cmdInsertContact.Parameters.AddWithValue("@PersonId", PersonId != 0 ? (object)PersonId : DBNull.Value);
                    cmdInsertContact.Parameters.AddWithValue("@contactName", !string.IsNullOrWhiteSpace(cName) ? (object)cName : DBNull.Value);
                    cmdInsertContact.Parameters.AddWithValue("@contactCellphone", p.Contact?.Cellphone?.Cellphone ?? (object)DBNull.Value);
                    cmdInsertContact.Parameters.AddWithValue("@contactRelation", !string.IsNullOrWhiteSpace(p.Contact?.Relation) ? (object)p.Contact.Relation : DBNull.Value);
                    cmdInsertContact.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public void Delete(Patient patient)
        {
            const string sql = @"DELETE FROM dbo.Patient WHERE PatientId = @patientId";
            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@patientId", patient.Id_patient ?? string.Empty);
            cmd.ExecuteNonQuery();
        }

        public Patient Search(Patient patient)
        {
            return FindById_patient(patient);
        }

        public Patient Search(Patient patient, bool dummy = false)
        {
            return FindById_patient(patient);
        }
    }
}