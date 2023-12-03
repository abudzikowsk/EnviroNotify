namespace EnviroNotify.Database.Entities;

public class PersistedClient
{
    public int Id { get; set; }
    public string ClientId { get; set; }
    
    public string Endpoint { get; set; }

    public string P256DH { get; set; }

    public string Auth { get; set; }
}