namespace Hospital.infraestructure
{
    public class Config
    {
        public static string SqlConnectionString =>
    "Server=(localdb)\\MSSQLLocalDB;" +
    "Database=HospitalDB;" +
    "Integrated Security=True;" +
    "Persist Security Info=False;" +
    "Pooling=False;" +
    "MultipleActiveResultSets=False;" +
    "Encrypt=True;" +
    "TrustServerCertificate=False;" +
    "Application Name=\"SQL Server Management Studio\";" +
    "Command Timeout=0;";

    }
}
