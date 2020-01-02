using System;
using System.Collections.Generic;
using System.Text;

namespace PluginCore
{
    public class PluginResultCodeDefines
    {
        public const string SuccessCode = "0";
        public const string ModelStateInvalid = "100";
        public const string ArgumentNullError = "101";
        public const string ObjectAlreadyExists = "102";

        public const string NotFound = "404";
        public const string NotAllow = "403";
        public const string ServiceError = "500";
    }
}
