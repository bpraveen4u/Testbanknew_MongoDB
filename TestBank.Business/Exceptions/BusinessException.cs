using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBank.Business.Exceptions
{
    public class BusinessException : Exception
    {
        #region CTORs
        public BusinessException(string message, List<string> errors)
            : base(message)
        {
            this._errors = errors.AsEnumerable();
        }
        public BusinessException(List<string> errors)
        {
            this._errors = errors.AsEnumerable();
        }
        public BusinessException(string message)
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
