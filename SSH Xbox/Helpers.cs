using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SSH_Xbox
{
    static class Helpers
    {
        public static String GetLastLines(this String text, int lines)
        {
            return String.Join("\n", text.Split('\n').GetLast(lines));
        }

        public static T[] GetLast<T>(this T[] source, int n)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (n > source.Length)
                return source;
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n), "Can't be negative");

            var target = new T[n];
            Array.Copy(source, source.Length - n, target, 0, n);
            return target;
        }
    }
}

namespace System.Runtime
{
    class StringExtensions
    {
    }
}
