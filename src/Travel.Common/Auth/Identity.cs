namespace Travel.Common.Auth
{
    public class Identity
    {
        public string Username { get; }
        public string Role { get; }

        public Identity(string username, string role)
        {
            Username = username;
            Role = role;
        }
    }
}
