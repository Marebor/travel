namespace Travel.Common.Auth
{
    public interface IIdentityProvider
    {
        Identity GetIdentity();
    }
}
