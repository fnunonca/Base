using System;
using System.Collections.Immutable;
using System.Net;

namespace ROP
{
    public struct Result<T>
    {
        public readonly T Value;

        public static implicit operator Result<T>(T value) => new Result<T>(value, HttpStatusCode.OK);

        public static implicit operator Result<T>(ImmutableArray<Error> errors) => new Result<T>(errors, HttpStatusCode.BadRequest);

        public static implicit operator Result<T>(Error error) => new Result<T>(error, HttpStatusCode.BadRequest);

        public readonly ImmutableArray<Error> Errors;
        public readonly Error Error;

        public readonly HttpStatusCode HttpStatusCode;
        public bool Success => Errors.Length == 0 && Error == null;

        public Result(Error error, HttpStatusCode statusCode)
        {
            HttpStatusCode = statusCode;
            Value = default(T);
            Error = error;
            Errors = ImmutableArray<Error>.Empty;
        }
        public Result(T value, HttpStatusCode statusCode)
        {
            Value = value;
            Errors = ImmutableArray<Error>.Empty;
            HttpStatusCode = statusCode;
            Error = default(Error);

        }

        public Result(ImmutableArray<Error> errors, HttpStatusCode statusCode)
        {
            if (errors.Length == 0)
            {
                throw new InvalidOperationException("You should specify at least one error");
            }

            HttpStatusCode = statusCode;
            Value = default(T);
            Errors = errors;
            Error = default(Error);

        }


    }
}

