using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;

namespace Bodoconsult.Core.Windows.Network.ActiveDirectory.Helpers
{

    /// <summary>
    /// Helper class with AD functionality
    /// </summary>
    public class AdHelper
    {
        /// <summary>
        /// Convert friendly domain name to LDAP domain path
        /// </summary>
        /// <param name="friendlyDomainName">friendly domain name</param>
        /// <returns></returns>

        public static string FriendlyDomainToLdapDomain(string friendlyDomainName)
        {
            string ldapPath;
            try
            {
                var bFirst = true;
                var sbReturn = new StringBuilder(200);
                var strlstDc = friendlyDomainName.Split('.');
                foreach (var strDc in strlstDc)
                {
                    if (bFirst)
                    {
                        sbReturn.Append("DC=");
                        bFirst = false;
                    }
                    else
                        sbReturn.Append(",DC=");

                    sbReturn.Append(strDc);
                }

                ldapPath = "LDAP://"+sbReturn;
            }
            catch
            {
                ldapPath = null;
            }
            return ldapPath;
        }


        /// <summary>
        /// Get the (user) domain for the current user
        /// </summary>
        /// <returns></returns>
        public static string GetLdapDomainForCurrentUser()
        {
            var domain = Domain.GetCurrentDomain().Name;

            return FriendlyDomainToLdapDomain(domain);
        }

        /// <summary>
        /// Get current AD domain name
        /// </summary>
        /// <returns>domain name</returns>
        public static string GetDomainName()
        {
            return Domain.GetCurrentDomain().Name;
        }



        /// <summary>
        /// Get detail data for the users
        /// </summary>
        /// <param name="domain">AD domain filled with user data</param>
        public static void GetUserData(AdDomain domain)
        {
            var ad = new PrincipalContext(ContextType.Domain, domain.FqdnName);
            var u = new UserPrincipal(ad);
            var search = new PrincipalSearcher(u);

            foreach (var item in search.FindAll())
            {

                var result = (UserPrincipal) item;

                var user = domain.Users.FirstOrDefault(x => x.UserPrincipalName == result.UserPrincipalName);

                if (user == null) continue;

                //Xmlc.CheckNode(value, CheckString(result.Name));

                //value = item + "/NetworkItemValue[DS_userPrincipalName]";
                //Xmlc.CheckNode(value, CheckString(result.UserPrincipalName));

                //value = item + "/NetworkItemValue[DS_distinguishedName]";
                //Xmlc.CheckNode(value, CheckString(result.DistinguishedName));

                user.FirstName = result.GivenName;

                user.Surname = result.Surname;

                user.MailAddress = result.EmailAddress;

                user.ScriptPath = result.ScriptPath;

                //user.HomeDirectory = CheckString(result.HomeDirectory);

                user.DontExpirePassword = result.PasswordNeverExpires;

                user.Disabled = result.Enabled==false;

                user.PasswordNotRequired = result.PasswordNotRequired;

                user.PasswordCantChange = result.UserCannotChangePassword;


                user.LastLogon = result.LastLogon;
                user.Sid = result.Sid.Value;


                //value = item + "/NetworkItemValue[DS_PasswordNotRequired]";
                //Xmlc.CheckNode(value, CheckBool(result.PasswordNotRequired);

                //value = item + "/NetworkItemValue[DS_PasswordCantChange]";
                //Xmlc.CheckNode(value, CheckBool();

                //value = item + "/NetworkItemValue[DS_lastLogon]";
                //Xmlc.CheckNode(value, result.LastLogon == null ? "" : CheckDate((DateTime)result.LastLogon);

                //value = item + "/NetworkItemValue[DS_Sid]";
                //Xmlc.CheckNode(value, CheckString(result.Sid.Value);
            }

            
        }
    }
}
