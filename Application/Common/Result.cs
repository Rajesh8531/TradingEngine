using Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common
{
    public class Result<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public IReadOnlyList<Error> Errors { get; set; } = [];
        public ErrorType ErrorType { get; set; }
        public T Value { get; set; } = null!;

        public static Result<T> Success(T value)
        {
            return new Result<T> { IsSuccess = true, Value = value };
        }

        public static Result<T> Failure(IReadOnlyList<Error> errors, ErrorType errorType)
        {
            return new Result<T> { IsSuccess = false, Errors = errors, ErrorType = errorType };
        }
    }
}
