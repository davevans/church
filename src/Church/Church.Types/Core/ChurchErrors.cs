using Church.Common.Structures;

namespace Church.Types.Core
{

    public sealed class ChurchErrors
    {
        public const string SystemCode = "C";
        
        public const ulong UNKNOWN = 0x0;
        public const ulong DUPLICATE_CHURCH_NAME = 0x1;

        public static Error UnknownError { get { return Error(UNKNOWN); } }
        public static Error DuplicateChurchName { get { return Error(DUPLICATE_CHURCH_NAME); } }


        public static Error Error(ulong code)
        {
            return new Error(SystemCode, code);
        }
    }
}
