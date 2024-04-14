namespace ProcureHub_ASP.NET_Core.Helper
{
    public interface IUserContext
    {
        public void SetName(string name);

        public void SetEmail(string email);
        public void SetUserId(int userId);

        public void SetPartnerCode(string partnerCode);

        public void SetIsSupplier(bool isSupplier);

        public void SetIsAdmin(bool isAdmin);

        public object GetUserContext();
        public string GetEmail();
        public bool GetIsSupplier();
        public void ExtractInfo(string token);
        public int GetUserId();
    }
}
