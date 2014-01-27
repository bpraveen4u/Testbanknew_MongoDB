using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBank.Entity.Errors
{
    public class ApiError: Exception
    {
        #region CTORs
        public ApiError(string message, List<string> errors)
            : base(message)
        {
            this._errors = errors.AsEnumerable();
        }
        public ApiError(List<string> errors)
            : base("Errors")
        {

            this._errors = errors.AsEnumerable();
        }
        public ApiError(string message)
            : base(message)
        {
        }
        #endregion

        #region Props
        private IEnumerable<string> _errors;
        public IEnumerable<string> Errors
        {
            get
            {
                return _errors;
            }
        }
        #endregion
    }
}
