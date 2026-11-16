using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.BLL.Common
{
    public sealed record Result(bool Success, string? Error = null, ResultKind Kind = ResultKind.OK)
    {
        public static Result Ok() => new Result(true);
        public static Result Fail(string error, ResultKind kind = ResultKind.Conflict) => new Result(false, error, kind);
        public static Result NotFound(string error = "Not Found") => new Result(false, error, ResultKind.NotFound);
        public static Result Validation(string error) => new Result(false, error, ResultKind.ValidationFailed);
    }

    public sealed record Result<T>(bool Success, T? Value, string? Error = null, ResultKind Kind = ResultKind.OK)
    {
        public static Result<T> Ok(T value) => new Result<T>(true, value);
        public static Result<T> Fail(string error, ResultKind kind = ResultKind.Conflict) => new Result<T>(false, default, error, kind);
        public static Result<T> NotFound(string error = "Not Found") => new Result<T>(false, default, error, ResultKind.NotFound);
    }

    public enum ResultKind
    {
        OK,
        NotFound,
        Conflict,
        ValidationFailed,
        Forbidden
    }
}
