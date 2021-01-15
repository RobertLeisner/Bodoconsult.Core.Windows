using System;
using System.Collections.Generic;

namespace Bodoconsult.Core.Windows.Network.ActiveDirectory
{
    /// <summary>
    /// Represents a AD user
    /// </summary>
    public class AdUser: IAdObject
    {

        /// <summary>
        /// Default ctor
        /// </summary>
        public AdUser()
        {
            MemberOf = new List<IAdObject>();
        }

        ///// <summary>
        ///// the distinguished name of the user
        ///// </summary>
        //public string DistinguishedName { get; set; }


        /// <summary>
        /// LDAP path to the user
        /// </summary>
        public string Path { get; set; }

        public string Name { get; set; }



        public string UserPrincipalName { get; set; }


        /// <summary>
        /// Groups, the user is a member of
        /// </summary>
        public IList<IAdObject> MemberOf { get; set; }


        /// <summary>
        /// Full name of the user
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Surname of the user
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// First name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// email address of the user
        /// </summary>
        public string MailAddress { get; set; }

        ///// <summary>
        ///// Distinguished name of the suer
        ///// </summary>

        //public string DistinguishedName { get; set; }

        ///// <summary>
        ///// Principal name of the suer
        ///// </summary>
        //public string PrincipalName { get; set; }

        /// <summary>
        /// Path to the user's profile
        /// </summary>
        public string ProfilePath { get; set; }
        /// <summary>
        /// Path to the user's logon script
        /// </summary>
        public string ScriptPath { get; set; }

        /// <summary>
        /// Date of the last logon
        /// </summary>
        public DateTime? LastLogon { get; set; }

        /// <summary>
        /// Groups the user is member of
        /// </summary>

        public List<string> Groups { get; set; }

        /// <summary>
        /// Direct permissions of the user
        /// </summary>
        public List<string> DirectPermissions { get; set; }

        /// <summary>
        /// Is user disabled?
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Password is not required for the user account
        /// </summary>
        public bool PasswordNotRequired { get; set; }

        /// <summary>
        /// User can't change password
        /// </summary>
        public bool PasswordCantChange { get; set; }

        /// <summary>
        /// User's password never expires
        /// </summary>
        public bool DontExpirePassword { get; set; }

        /// <summary>
        /// SID of the user
        /// </summary>
        public string Sid { get; set; }

    }
}