using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public IReadOnlyList<Error> Errors { get; set; }

        public Result(bool isSuccess, IReadOnlyList<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Ok() => new(true, Array.Empty<Error>());
        public static Result Fail(Error error) => new(false, new[] { error });
        public static Result Fail(IReadOnlyList<Error> errors) => new(false, errors);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public TValue data => IsSuccess ? _value : throw new InvalidOperationException("Failed To Get data !");
        private Result(TValue value) : base(true, Array.Empty<Error>())
        {
            _value = value;
        }
        private Result(Error error) : base(false, new[] { error })
        {
            _value = default!;
        }
        private Result(IReadOnlyList<Error> errors) : base(false, errors)
        {
            _value = default!;
        }

        public static Result<TValue> Ok(TValue value) => new(value);
        public static Result<TValue> Fail(Error error) => new(new[] { error });
        public static Result<TValue> Fail(IReadOnlyList<Error> errors) => new(errors);
    }
}
