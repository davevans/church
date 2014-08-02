namespace Church.Common.Structures
{
    public class Error
    {
        public string SystemCode { get; set; }
        public ulong Code { get; set; }

        public Error(string systemCode, ulong code)
        {
            SystemCode = systemCode;
            Code = code;
        }
    }
}
