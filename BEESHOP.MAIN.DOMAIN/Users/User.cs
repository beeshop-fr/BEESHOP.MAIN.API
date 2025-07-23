namespace BEESHOP.MAIN.DOMAIN.Users;

public class User
{
    public Guid id { get; set; }
    public string nom { get; set; }
    public string mdp { get; set; }
    public string email { get; set; }
    public string role { get; set; }
}
