namespace Asp.NetCore.SecuredSpa.Security
{
    public interface IIdentityResolver
    {
        bool IsIdentityConfirmed(string userName, string password);
    }
}
