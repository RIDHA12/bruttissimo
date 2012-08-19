﻿using System;

namespace Bruttissimo.Common
{
    public static class GuidExtensions
    {
        public static string Stringify(this Guid guid)
        {
            return guid.ToString().Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
