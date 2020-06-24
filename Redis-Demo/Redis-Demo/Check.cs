using System;

namespace Redis_Demo
{
    public static class Check
    {
        public static void Requires<TException>(bool isTrue, string message) where TException : Exception
        {
            if (isTrue) return;
            var exception = Activator.CreateInstance(typeof(TException), message) as TException;
            if (exception != null)
                throw exception;
        }
    }
}
