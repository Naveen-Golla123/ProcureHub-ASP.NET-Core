namespace ProcureHub_ASP.NET_Core.Helper
{
    public class UserContext : IUserContext
    {

        private string name;
        private string email;
        private bool isSupplier = false;
        private string partnerCode;
        private bool isAdmin = false;

        public UserContext() 
        {
            
        }   

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetEmail(string email)
        {
            this.email = email;
        }

        public void SetPartnerCode(string partnerCode)
        {
            this.partnerCode = partnerCode;
        }

        public void SetIsSupplier(bool isSupplier)
        {
            this.isSupplier = isSupplier;
        }

        public void SetIsAdmin(bool isAdmin)
        {
            this.isAdmin = isAdmin;
        }

        public object GetUserContext()
        {
            return new
            {
                name = ""
            };
        }
    }
}
