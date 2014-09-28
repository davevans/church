namespace Church.Common.Structures
{
    public class Result<T>
    {
        public Error Error { get; set; }
        public T Value { get; set; }

        public bool IsSuccess
        {
            get { return Error == null; }
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>
            {
                Value = value
            };
        }

        public static Result<T> Failure(Error error)
        {
            return new Result<T>
            {
                Error = error,
            };
        }
    }

    public class Result
    {
        public Error Error { get; set; }
        public bool IsSuccess
        {
            get { return Error == null; }
        }

        public static Result Success()
        {
            return new Result();
        }

        public static Result Failure(Error error)
        {
            return new Result
            {
                Error = error,
            };
        }
    }
}
