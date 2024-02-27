namespace ProcureHub_ASP.NET_Core.Helper
{
    public interface IUserContext
    {
        public void SetName(string name);

        public void SetEmail(string email);

        public void SetPartnerCode(string partnerCode);

        public void SetIsSupplier(bool isSupplier);

        public void SetIsAdmin(bool isAdmin);

        public object GetUserContext();
    }
}
