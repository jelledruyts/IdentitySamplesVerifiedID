namespace ContosoMusiversity;

public class AppConfiguration
{
    public string? StudentAppRoleName { get; set; }
    public string? StudentCredentialType { get; set; }
    public string? StaffAppRoleName { get; set; }
    public string? StaffCredentialType { get; set; }
    public IList<string> VerifiedCredentialInputClaims { get; set; } = new List<string>();
}