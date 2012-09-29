namespace Bruttissimo.Common.Mvc.InversionOfControl.Membership
{
    public class DefaultMembershipProvider : IMembershipProvider
    {
        public bool ValidateUser(string username, string password)
        {
            return System.Web.Security.Membership.ValidateUser(username, password);
        }
    }
}
