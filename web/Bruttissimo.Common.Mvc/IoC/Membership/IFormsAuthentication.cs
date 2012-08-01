namespace Bruttissimo.Common.Mvc
{
	public interface IFormsAuthentication
	{
		void SetAuthCookie(string username, bool createPersistentCookie);
		void SetAuthCookie(string username, bool createPersistentCookie, string cookiePath);
		void SignOut();
	}
}