namespace Ogu.Extensions.SafeResult
{
    public interface ISafeResult<out TType>
    {
        TType Result { get; }

        bool IsThereAnyFailure { get; }

        bool StopOnFailure { get; }

        int SuccessCount { get; }

        int FailureCount { get; }
    }
}