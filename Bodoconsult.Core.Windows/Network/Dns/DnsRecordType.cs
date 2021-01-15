namespace Bodoconsult.Core.Windows.Network.Dns
{
    /// <summary>
    /// The type of record in Microsoft (r) DNS server
    /// </summary>
    public class DnsRecordType
    {
        /// <summary>
        /// Create a new DNS record type
        /// </summary>
        /// <param name="textRepresentation">The type to create</param>
        public DnsRecordType(string textRepresentation)
        {
            _textRepresentation = textRepresentation;
        }
        private readonly string _textRepresentation = "";

        /// <summary>
        /// The text representation of the record type
        /// </summary>
        public string TextRepresentation => _textRepresentation.ToUpper();

        /// <summary>
        /// The mode of the record, usually IN but could oneday be something else like OUT
        /// </summary>
        public string RecordMode { get; set; } = "IN";

        public override string ToString()
        {
            return RecordMode + " " + _textRepresentation;
        }

        #region Default values
        /// <summary>
        /// An alias
        /// </summary>
        public static DnsRecordType Cname => new DnsRecordType("CNAME");

        /// <summary>
        /// An IPv4 address
        /// </summary>
        public static DnsRecordType A => new DnsRecordType("A");

        /// <summary>
        /// A reverse host address inside yoursubnet.in-addr.arpa
        /// </summary>
        public static DnsRecordType Ptr => new DnsRecordType("PTR");

        /// <summary>
        /// An MX record (mail exchange)
        /// </summary>
        public static DnsRecordType Mx => new DnsRecordType("MX");

        /// <summary>
        /// An IPv6 host address
        /// </summary>
        public static DnsRecordType Aaaa => new DnsRecordType("AAAA");

        /// <summary>
        /// A text record
        /// </summary>
        public static DnsRecordType Txt => new DnsRecordType("TXT");

        /// <summary>
        /// A nameserver record (domain delegation)
        /// </summary>
        public static DnsRecordType Ns => new DnsRecordType("NS");

        /// <summary>
        /// An SOA record (start of authority)
        /// </summary>
        public static DnsRecordType Soa => new DnsRecordType("SOA");

        #endregion
    }
}