using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Utilites.Services
{
    public class OperationResult<T>
    {
        public bool Success { get; private set; }
        public T? Data { get; private set; }
        public string? ErrorMessage { get; private set; }
        public string? SuccessMessage { get; private set; }

        public static OperationResult<T> SuccessResult(T data, string successMessage = null) =>
        new OperationResult<T> { Success = true, Data = data, SuccessMessage = successMessage };

        public static OperationResult<T> FailureResult(string errorMessage) =>
            new OperationResult<T> { Success = false, ErrorMessage = errorMessage };
    }
}
