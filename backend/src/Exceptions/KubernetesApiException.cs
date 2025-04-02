namespace Exceptions
{
    public class KubernetesApiException : Exception
    {
        public int StatusCode { get; }

        public KubernetesApiException(string message, int statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public KubernetesApiException(string message, int statusCode, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}