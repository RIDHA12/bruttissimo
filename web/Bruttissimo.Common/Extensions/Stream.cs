﻿using System.IO;

namespace Bruttissimo.Common.Extensions
{
    public static class StreamExtensions
    {
        public static string ReadFully(this Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
