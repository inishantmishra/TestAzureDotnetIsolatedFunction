using FluentResults;
using System.Diagnostics.CodeAnalysis;

namespace TestFunction;

public class NonCriticalException : Exception, IError
{
    public NonCriticalException(string message) : base(message)
    {

    }

    [ExcludeFromCodeCoverage]
    public List<IError> Reasons => throw new NotImplementedException();
    [ExcludeFromCodeCoverage]
    public Dictionary<string, object> Metadata => throw new NotImplementedException();
}
