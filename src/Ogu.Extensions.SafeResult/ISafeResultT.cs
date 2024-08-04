namespace Ogu.Extensions.SafeResult
{
    public interface ISafeResult<out TType>
    {
        TType Result { get; }

        bool HasFailure { get; }

        bool StopOnFailure { get; }

        int SuccessCount { get; }

        int FailureCount { get; }
    }
}