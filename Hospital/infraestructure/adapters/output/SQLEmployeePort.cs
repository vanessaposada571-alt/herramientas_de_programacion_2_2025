using Hospital.domain.ports;
using Hospital.domain.model;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Net.Mail;

namespace Hospital.infraestructure.adapters.output
{
    public class SqlEmployeePort : Employee_port
    {
        private SqlConnection GetConn() => new SqlConnection(Config.SqlConnectionString);

        public User FindById1(User user)
        {
            if (user == null) return null;

            const string sql = @"
        SELECT 
            u.UserId, u.Role, u.UserName, u.PasswordHash
        FROM dbo.Users u
        WHERE u.UserName = @userName";

            using var cn = GetConn();
            cn.Open();

            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@userName", user.Name_user);

            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            return rd.Read() ? MapUser(rd) : null;
        }


        public User FindByName_user(User user)
        {
            if (user == null) return null;

            const string sql = @"
        SELECT UserId, Role, UserName, PasswordHash
        FROM dbo.Users
        WHERE UserName = @userName";

            using var cn = GetConn();
            cn.Open();

            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@userName", user.Name_user ?? string.Empty);

            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            return rd.Read() ? MapUser(rd) : null;
        }


        public void Save(User user)
        {
            const string insertUser = @"
                INSERT INTO users (Role, UserName, PasswordHash)
                VALUES (@role, @userName, @passwordHash);
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

            using var cn = GetConn();
            cn.Open();

            // Validación: UserName no repetido (comparación trim + LOWER)
            var userNameToCheck = user.Name_user?.Trim();
            if (!string.IsNullOrWhiteSpace(userNameToCheck))
            {
                const string checkSql = @"
                    SELECT COUNT(1)
                    FROM dbo.Users
                    WHERE LOWER(LTRIM(RTRIM(UserName))) = LOWER(LTRIM(@userName))";
                using var checkCmd = new SqlCommand(checkSql, cn);
                checkCmd.Parameters.AddWithValue("@userName", userNameToCheck);
                var exists = Convert.ToInt32(checkCmd.ExecuteScalar() ?? 0) > 0;
                if (exists) throw new Exception("El UserName ya está en uso.");
            }

            using var tx = cn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(insertUser, cn, tx);
                cmd.Parameters.AddWithValue("@role", (object)(user.Rol ?? string.Empty));
                cmd.Parameters.AddWithValue("@userName", (object)(user.Name_user ?? string.Empty));
                cmd.Parameters.AddWithValue("@passwordHash", (object)(user.Password ?? string.Empty));

                var userIdObj = cmd.ExecuteScalar();
                if (userIdObj == null || userIdObj == DBNull.Value)
                    throw new Exception("No se pudo obtener UserId tras insertar Users.");

                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                MessageBox.Show("Error al guardar el usuario: " + ex.Message);
                throw;
            }
        }

        public void Update(User existingUser)
        {
            const string updatePerson = @"
                UPDATE dbo.Person
                SET Name = @name, Email = @email, Cellphone = @cellphone, BirthDate = @birthDate, Gender = @gender, Direction = @direction
                WHERE IdNumber = @idNumber;";

            const string updateUser = @"
                UPDATE dbo.Users
                SET Role = @role, UserName = @userName, PasswordHash = @passwordHash
                WHERE PersonId = (SELECT PersonId FROM dbo.Person WHERE IdNumber = @idNumber);";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(updatePerson + updateUser, cn);
            cmd.Parameters.AddWithValue("@name", existingUser.Name ?? string.Empty);
            cmd.Parameters.AddWithValue("@email", !string.IsNullOrWhiteSpace(existingUser.Email?.Address) ? (object)existingUser.Email.Address : DBNull.Value);
            cmd.Parameters.AddWithValue("@cellphone", existingUser.Cellphone != 0 ? (object)existingUser.Cellphone : DBNull.Value);
            cmd.Parameters.AddWithValue("@birthDate", (object)(existingUser.Birth == default ? DBNull.Value : existingUser.Birth));

            // Normalizar género: aceptar texto tal como lo escribe el cliente ("masculino" / "femenino")
            var genderNormalized = NormalizeGender(existingUser.Gender);
            cmd.Parameters.AddWithValue("@gender", genderNormalized != null ? (object)genderNormalized : DBNull.Value);

            cmd.Parameters.AddWithValue("@direction", !string.IsNullOrWhiteSpace(existingUser.Direction) ? (object)existingUser.Direction : DBNull.Value);
            cmd.Parameters.AddWithValue("@idNumber", existingUser.Id);
            cmd.Parameters.AddWithValue("@role", existingUser.Rol ?? string.Empty);
            cmd.Parameters.AddWithValue("@userName", existingUser.Name_user ?? string.Empty);
            cmd.Parameters.AddWithValue("@passwordHash", existingUser.Password ?? string.Empty);
            cmd.ExecuteNonQuery();
        }

        public void Delete(User user)
        {
            // Borra usuario y persona asociada (cascada definida en la BD si se desea)
            const string deleteUser = @"
                DELETE FROM dbo.Users WHERE PersonId = (SELECT PersonId FROM dbo.Person WHERE IdNumber = @idNumber);
                DELETE FROM dbo.Person WHERE IdNumber = @idNumber;";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(deleteUser, cn);
            cmd.Parameters.AddWithValue("@idNumber", user.Id);
            cmd.ExecuteNonQuery();
        }

        public void Search(User user)
        {
            // No implementado: la consulta depende de criterios
            throw new NotImplementedException();
        }

        private static User MapUser(SqlDataReader rd)
        {
            var u = new User();
            u.Name = rd["Name"]?.ToString() ?? string.Empty;
            u.Id = rd["IdNumber"] != DBNull.Value ? Convert.ToInt64(rd["IdNumber"]) : 0L;
            var emailStr = rd["Email"]?.ToString();
            u.Email = !string.IsNullOrWhiteSpace(emailStr) ? new MailAddress(emailStr) : null;
            u.Cellphone = rd["Cellphone"] != DBNull.Value ? Convert.ToInt64(rd["Cellphone"]) : 0L;

            // Mapear y normalizar género para que sea "masculino" o "femenino" si es posible
            u.Gender = NormalizeGender(rd["Gender"]?.ToString());

            u.Rol = rd["Role"]?.ToString() ?? string.Empty;
            u.Name_user = rd["UserName"]?.ToString() ?? string.Empty;
            u.Password = rd["PasswordHash"]?.ToString() ?? string.Empty;
            u.Direction = rd["Direction"]?.ToString() ?? string.Empty;
            return u;
        }

        // Helper: normaliza variantes comunes y devuelve "masculino" o "femenino" si se reconoce.
        // Si no se reconoce y el valor es no vacío, devuelve el valor saneado en minúsculas; si está vacío, devuelve null.
        private static string NormalizeGender(string gender)
        {
            if (string.IsNullOrWhiteSpace(gender)) return null;
            var g = gender.Trim().ToLowerInvariant();
            if (g == "m" || g == "masculino" || g == "male" || g == "masc" || g == "Masculino") return "masculino";
            if (g == "f" || g == "femenino" || g == "female" || g == "fem" || g == "Femenino") return "femenino";
            return g; // devolver la forma saneada para flexibilidad
        }
    }
}