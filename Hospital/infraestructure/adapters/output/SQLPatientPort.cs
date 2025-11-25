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
            using (var conn = GetConn())
            using (var cmd = new SqlCommand(@"
SELECT TOP(1)
       p.PatientId, p.PolicyNumber,
       per.PersonId, per.Name, per.IdNumber, per.Email, per.Cellphone, per.BirthDate, per.Gender, per.Direction,
       mi.MedicalInsuranceId, mi.CompanyName, mi.PolicyNumber AS MiPolicyNumber, mi.EffectiveDate,
       c.ContactId, c.PersonId AS ContactPersonId, c.Name AS ContactName, c.Cellphone AS ContactCellphone, c.Relation AS ContactRelation
FROM dbo.Patient p
LEFT JOIN dbo.Person per ON p.PersonId = per.PersonId
LEFT JOIN dbo.MedicalInsurance mi ON p.MedicalInsuranceId = mi.MedicalInsuranceId
LEFT JOIN dbo.Contact c ON c.PatientId = p.PatientId
WHERE p.PatientId = @patientId", conn))
            {
                // Corregido: no enviar string vacío, usar DBNull.Value
                if (string.IsNullOrWhiteSpace(patient.Id_patient))
                    cmd.Parameters.AddWithValue("@patientId", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@patientId", patient.Id_patient);

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    var found = new Patient
                    {
                        Id_patient = reader["PatientId"]?.ToString(),
                        Name = reader["Name"]?.ToString(),
                        Email = reader["Email"] != DBNull.Value ? new MailAddress(reader["Email"].ToString()) : null,
                        Cellphone = reader["Cellphone"] != DBNull.Value ? Convert.ToInt64(reader["Cellphone"]) : 0,
                        Birth = reader["BirthDate"] != DBNull.Value ? Convert.ToDateTime(reader["BirthDate"]) : default,
                        Direction = reader["Direction"]?.ToString(),
                        Gender1 = new Person { Gender = reader["Gender"]?.ToString() }
                    };

                    // Contacto (corregido: usar ContactRelation)
                    if (reader["ContactId"] != DBNull.Value)
                    {
                        found.Contact = new Contact
                        {
                            Name = reader["ContactName"]?.ToString(),
                            Relation = reader["ContactRelation"] != DBNull.Value
                                        ? reader["ContactRelation"].ToString()
                                        : string.Empty,
                            Cellphone = new Person
                            {
                                Cellphone = reader["ContactCellphone"] != DBNull.Value
                                            ? Convert.ToInt64(reader["ContactCellphone"])
                                            : 0
                            }
                        };
                    }

                    // Añadir esta línea para mantener el wrapper consistente
                    found.Cellphone1 = new Person
                    {
                        Cellphone = reader["Cellphone"] != DBNull.Value ? Convert.ToInt64(reader["Cellphone"]) : 0
                    };

                    // Nuevo: mapear datos de póliza asociados al paciente
                    // Priorizar el MedicalInsuranceId de la JOIN; si existe, poblar IdSure y PolicyNumber
                    if (reader["MedicalInsuranceId"] != DBNull.Value)
                    {
                        found.IdSure = new Medical_insurance
                        {
                            IdSure = reader["MedicalInsuranceId"] != DBNull.Value ? Convert.ToInt32(reader["MedicalInsuranceId"]) : 0,
                            Company_name = reader["CompanyName"]?.ToString() ?? string.Empty,
                            Policy_number = reader["MiPolicyNumber"]?.ToString() ?? string.Empty,
                            Effective_Date = reader["EffectiveDate"] != DBNull.Value ? Convert.ToDateTime(reader["EffectiveDate"]) : default
                        };

                        // Guardar también el número de póliza en el wrapper patient.PolicyNumber por compatibilidad
                        found.PolicyNumber = reader["MiPolicyNumber"]?.ToString() ?? reader["PolicyNumber"]?.ToString() ?? string.Empty;
                    }
                    else
                    {
                        // Si no hay join a MedicalInsurance, tomar el PolicyNumber desde la tabla Patient (p.PolicyNumber)
                        found.PolicyNumber = reader["PolicyNumber"]?.ToString() ?? string.Empty;
                    }

                    return found;
                }
            }
        }

        // Nuevo: busca por IdNumber (campo de la tabla Person) y mapea todos los datos al domain Patient
        public Patient FindByIdNumber(Patient patient)
        {
            if (patient == null) return null;
            const string sql = @"
                SELECT TOP(1)
                       p.PatientId, p.PolicyNumber,
                       per.PersonId, per.Name , per.IdNumber, per.Email, per.Cellphone, per.BirthDate, per.Gender, per.Direction,
                       mi.MedicalInsuranceId, mi.CompanyName, mi.PolicyNumber AS MiPolicyNumber, mi.EffectiveDate,
                       c.ContactId, c.PersonId AS ContactPersonId, c.Name AS ContactName, c.Cellphone AS ContactCellphone, c.Relation AS ContactRelation
                FROM dbo.Patient p
                LEFT JOIN dbo.Person per ON p.PersonId = per.PersonId
                LEFT JOIN dbo.MedicalInsurance mi ON p.MedicalInsuranceId = mi.MedicalInsuranceId
                LEFT JOIN dbo.Contact c ON c.PatientId = p.PatientId
                WHERE per.IdNumber = @idNumber";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            // Usamos patient.Id_patient como entrada del IdNumber desde la UI (txtIdPatient)
            cmd.Parameters.AddWithValue("@idNumber", patient.Id_patient ?? string.Empty);
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            var res = new Patient();
            res.Id_patient = rd["PatientId"]?.ToString();

            var name = rd["Name"]?.ToString();
            if (!string.IsNullOrWhiteSpace(name)) res.Name1 = new Person { Name = name };

            // Gender viene como string en BD; guardamos el texto (normalizado)
            var genderStr = rd["Gender"]?.ToString();
            if (!string.IsNullOrWhiteSpace(genderStr))
            {
                var genderNormalized = genderStr.Trim();
                res.Gender1 = new Person { Gender = genderNormalized };
            }

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

            // Validación: nombre o idNumber ya existente (comparación trim + LOWER para name)
            var nameToCheck = patient.Name?.Trim();
            var idNumberToCheck = patient.Id_patient?.Trim();
            if (!string.IsNullOrWhiteSpace(nameToCheck) || !string.IsNullOrWhiteSpace(idNumberToCheck))
            {
                var checkSql = @"
                    SELECT COUNT(1)
                    FROM dbo.Person
                    WHERE (LOWER(LTRIM(RTRIM(Name))) = LOWER(LTRIM(RTRIM(@name))) AND @name IS NOT NULL)
                       OR (IdNumber = @idNumber AND @idNumber IS NOT NULL)";

                using var checkCmd = new SqlCommand(checkSql, cn);
                checkCmd.Parameters.AddWithValue("@name", string.IsNullOrWhiteSpace(nameToCheck) ? (object)DBNull.Value : nameToCheck);
                checkCmd.Parameters.AddWithValue("@idNumber", string.IsNullOrWhiteSpace(idNumberToCheck) ? (object)DBNull.Value : idNumberToCheck);
                var exists = Convert.ToInt32(checkCmd.ExecuteScalar() ?? 0) > 0;
                if (exists) throw new Exception("Ya existe un registro con el mismo nombre o número de identificación.");
            }

            // Generar patientId una sola vez y reutilizarlo en Patient y Contact
            var patientId = string.IsNullOrWhiteSpace(patient.Id_patient) ? Guid.NewGuid().ToString() : patient.Id_patient;

            using var tx = cn.BeginTransaction();
            try
            {
                // Insert Person (todos los parámetros correctos)
                using var cmd1 = new SqlCommand(insertPerson, cn, tx);
                cmd1.Parameters.AddWithValue("@name", (object)(patient.Name ?? string.Empty));
                cmd1.Parameters.AddWithValue("@idNumber", !string.IsNullOrWhiteSpace(patient.Id_patient) ? (object)patient.Id_patient : DBNull.Value);

                // Email: preferir wrapper Email1 (compatibilidad) o Email
                var emailAddress = patient.Email1?.Email?.Address ?? patient.Email?.Address;
                cmd1.Parameters.AddWithValue("@email", !string.IsNullOrWhiteSpace(emailAddress) ? (object)emailAddress : DBNull.Value);

                // Cellphone: preferir wrapper Cellphone1
                var cellphone = patient.Cellphone1?.Cellphone ?? (patient.Cellphone != 0 ? patient.Cellphone : (long?)null);
                cmd1.Parameters.AddWithValue("@cellphone", cellphone != null ? (object)cellphone : DBNull.Value);

                // BirthDate
                cmd1.Parameters.AddWithValue("@birthDate", (object)(patient.Birth == default ? DBNull.Value : patient.Birth));

                // Gender: ahora es string; enviar el texto si existe
                string? genderText = null;
                if (patient.Gender1 != null && !string.IsNullOrWhiteSpace(patient.Gender1.Gender))
                    genderText = patient.Gender1.Gender.Trim();
                cmd1.Parameters.AddWithValue("@gender", !string.IsNullOrWhiteSpace(genderText) ? (object)genderText : DBNull.Value);

                // Direction: preferir wrapper Direction1 o propiedad Direction
                var direction = patient.Direction1?.Direction ?? patient.Direction;
                cmd1.Parameters.AddWithValue("@direction", !string.IsNullOrWhiteSpace(direction) ? (object)direction : DBNull.Value);

                var personIdObj = cmd1.ExecuteScalar();
                if (personIdObj == null || personIdObj == DBNull.Value) throw new Exception("No se obtuvo PersonId al insertar Person.");
                var personId = Convert.ToInt64(personIdObj);

                // Insert Patient (usa patientId calculado)
                using var cmd2 = new SqlCommand(insertPatient, cn, tx);
                cmd2.Parameters.AddWithValue("@patientId", patientId);
                cmd2.Parameters.AddWithValue("@personId", personId);
                cmd2.Parameters.AddWithValue("@medicalInsuranceId", patient.IdSure?.IdSure ?? (object)DBNull.Value);
                cmd2.Parameters.AddWithValue("@policyNumber", patient.PolicyNumber ?? (object)DBNull.Value);
                cmd2.ExecuteNonQuery();

                // Insertar contacto de emergencia si se proporcionó
                if (patient.Contact != null)
                {
                    using var cmd3 = new SqlCommand(insertContact, cn, tx);
                    cmd3.Parameters.AddWithValue("@patientId", patientId);

                    // personId del contacto: si Contact.PersonId > 0 usarlo, si no usar personId del paciente
                    cmd3.Parameters.AddWithValue("@personId", patient.Contact.PersonId != 0 ? (object)patient.Contact.PersonId : (object)personId);

                    var cName = patient.Contact.Name1?.Name ?? patient.Contact.Name;
                    cmd3.Parameters.AddWithValue("@contactName", !string.IsNullOrWhiteSpace(cName) ? (object)cName : DBNull.Value);

                    var contactPhone = patient.Contact.Cellphone?.Cellphone ?? (long?)null;
                    cmd3.Parameters.AddWithValue("@contactCellphone", contactPhone != null ? (object)contactPhone : DBNull.Value);

                    cmd3.Parameters.AddWithValue("@contactRelation", !string.IsNullOrWhiteSpace(patient.Contact.Relation) ? (object)patient.Contact.Relation : DBNull.Value);
                    cmd3.ExecuteNonQuery();
                }

                tx.Commit();

                // Asignar el patientId generado al objeto domain para que la UI lo vea
                patient.Id_patient = patientId;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public void Update(Patient patient)
        {
            if (string.IsNullOrWhiteSpace(patient.Id_patient))
                throw new Exception("El PatientId es obligatorio para actualizar.");

            using var cn = GetConn();
            cn.Open();

            // 1️⃣ Cargar datos actuales
            var existing = FindById_patient(patient); // ← CORREGIDO
            if (existing == null)
                throw new Exception("No existe el paciente con ese ID.");

            // 2️⃣ Completar valores nulos para no borrar información existente
            patient.Name ??= existing.Name;
            patient.Email1 ??= existing.Email1;

            // ✔ CORRECCIÓN: aseguramos que Cellphone1 exista y tenga valor
            if (patient.Cellphone1 == null || patient.Cellphone1.Cellphone == 0)
                patient.Cellphone1 = existing.Cellphone1;

            patient.Direction1 ??= existing.Direction1;
            patient.Gender1 ??= existing.Gender1;

            if (patient.Birth == default)
                patient.Birth = existing.Birth;

            patient.IdSure ??= existing.IdSure;
            patient.PolicyNumber ??= existing.PolicyNumber;

            patient.Contact ??= existing.Contact;

            using var tx = cn.BeginTransaction();

            try
            {
                // 3️⃣ Update Person
                const string updatePerson = @"
UPDATE dbo.Person
SET Name = @name, 
    Email = @email, 
    Cellphone = @cellphone,
    BirthDate = @birthDate, 
    Gender = @gender, 
    Direction = @direction
WHERE PersonId = (SELECT PersonId FROM dbo.Patient WHERE PatientId = @patientId);";

                using var cmd1 = new SqlCommand(updatePerson, cn, tx);
                cmd1.Parameters.AddWithValue("@patientId", patient.Id_patient);

                cmd1.Parameters.AddWithValue("@name", patient.Name ?? (object)DBNull.Value);

                var emailAddress = patient.Email1?.Email?.Address ?? patient.Email?.Address;
                cmd1.Parameters.AddWithValue("@email",
                    string.IsNullOrWhiteSpace(emailAddress) ? (object)DBNull.Value : emailAddress);

                // Ajuste: usar long? y mapear 0 a DBNull para no escribir 0 en BD
                string cellphone = patient.Cellphone.ToString();

                cmd1.Parameters.AddWithValue("@cellphone",
                    !string.IsNullOrWhiteSpace(cellphone) ? (object)cellphone : DBNull.Value);

                cmd1.Parameters.AddWithValue("@birthDate",
                    patient.Birth == default ? (object)DBNull.Value : patient.Birth);

                var gender = patient.Gender1?.Gender;
                cmd1.Parameters.AddWithValue("@gender",
                    string.IsNullOrWhiteSpace(gender) ? (object)DBNull.Value : gender);

                var direction = patient.Direction1?.Direction ?? patient.Direction;
                cmd1.Parameters.AddWithValue("@direction",
                    string.IsNullOrWhiteSpace(direction) ? (object)DBNull.Value : direction);

                cmd1.ExecuteNonQuery();


                // 4️⃣ Obtener PersonId
                const string sqlGetPid = "SELECT PersonId FROM dbo.Patient WHERE PatientId = @pid";
                long personId;

                using (var cmdu = new SqlCommand(sqlGetPid, cn, tx))
                {
                    cmdu.Parameters.AddWithValue("@pid", patient.Id_patient);
                    personId = Convert.ToInt64(cmdu.ExecuteScalar());
                }

                // 5️⃣ Update Patient
                const string updatePatient = @"
UPDATE dbo.Patient
SET MedicalInsuranceId = @insuranceId,
    PolicyNumber = @policy
WHERE PatientId = @patientId;";

                using var cmd2 = new SqlCommand(updatePatient, cn, tx);
                cmd2.Parameters.AddWithValue("@patientId", patient.Id_patient);
                cmd2.Parameters.AddWithValue("@insuranceId",
                    patient.IdSure?.IdSure ?? (object)DBNull.Value);
                cmd2.Parameters.AddWithValue("@policy",
                    patient.PolicyNumber ?? (object)DBNull.Value);
                cmd2.ExecuteNonQuery();


                // 6️⃣ Update Contact
                const string updateContact = @"
UPDATE dbo.Contact
SET PersonId = @personId,
    Name = @contactName,
    Cellphone = @contactCellphone,
    Relation = @relation
WHERE PatientId = @patientId;";

                var cName = patient.Contact?.Name1?.Name ?? patient.Contact?.Name;

                using var cmd3 = new SqlCommand(updateContact, cn, tx);
                cmd3.Parameters.AddWithValue("@patientId", patient.Id_patient);

                // Usar Contact.PersonId si se proporcionó (no 0), si no usar el personId del paciente
                var contactPersonId = patient.Contact?.PersonId != 0 ? patient.Contact.PersonId : personId;
                cmd3.Parameters.AddWithValue("@personId", contactPersonId != 0 ? (object)contactPersonId : (object)DBNull.Value);

                cmd3.Parameters.AddWithValue("@contactName",
                    string.IsNullOrWhiteSpace(cName) ? (object)DBNull.Value : cName);

                // Mapear cellphone del contacto: tratar 0 como NULL y fallback a existing.Contact
                long? contactCell = patient.Contact?.Cellphone?.Cellphone
                                    ?? existing.Contact?.Cellphone?.Cellphone
                                    ?? (long?)null;
                if (contactCell == 0) contactCell = null;
                cmd3.Parameters.AddWithValue("@contactCellphone",
                    contactCell != null ? (object)contactCell.Value : DBNull.Value);

                cmd3.Parameters.AddWithValue("@relation",
                    string.IsNullOrWhiteSpace(patient.Contact?.Relation)
                        ? (object)DBNull.Value
                        : patient.Contact.Relation);

                var affected = cmd3.ExecuteNonQuery();

                // 7️⃣ Insertar contacto si no existía
                if (affected == 0 && patient.Contact != null)
                {
                    const string insertContact = @"
INSERT INTO dbo.Contact (PatientId, PersonId, Name, Cellphone, Relation)
VALUES (@patientId, @personId, @contactName, @contactCellphone, @relation);";

                    using var cmd4 = new SqlCommand(insertContact, cn, tx);
                    cmd4.Parameters.AddWithValue("@patientId", patient.Id_patient);
                    cmd4.Parameters.AddWithValue("@personId", contactPersonId != 0 ? (object)contactPersonId : (object)DBNull.Value);
                    cmd4.Parameters.AddWithValue("@contactName",
                        string.IsNullOrWhiteSpace(cName) ? (object)DBNull.Value : cName);

                    long? contactCell2 = patient.Contact?.Cellphone?.Cellphone ?? existing.Contact?.Cellphone?.Cellphone ?? (long?)null;
                    if (contactCell2 == 0) contactCell2 = null;
                    cmd4.Parameters.AddWithValue("@contactCellphone",
                        contactCell2 != null ? (object)contactCell2.Value : DBNull.Value);

                    cmd4.Parameters.AddWithValue("@relation",
                        string.IsNullOrWhiteSpace(patient.Contact?.Relation)
                            ? (object)DBNull.Value
                            : patient.Contact.Relation);

                    cmd4.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
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