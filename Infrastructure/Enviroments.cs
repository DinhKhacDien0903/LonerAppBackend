using System;

namespace Infrastructure;

public static class Enviroments
{
    public static string ConnectionString_Docker = "Server=localhost, 1433; Database=Loner;User Id=sa;Password=DinhKhacDien1009!@#;TrustServerCertificate=True;";
    public static string ConnectionString_SSMS = "Server=localhost, 1433; Initial Catalog=Loner; Integrated Security=True; TrustServerCertificate=True;";
}
