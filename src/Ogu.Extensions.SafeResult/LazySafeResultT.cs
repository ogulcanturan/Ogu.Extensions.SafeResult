using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Ogu.Extensions.SafeResult
{
    public class LazySafeResult<T, TType> : ISafeResult<T>
    {
        private readonly string _elements;
        private readonly SafeResultType _type;
        private readonly bool _stopOnFailure;
        private readonly char[] _separators;
        private readonly IEqualityComparer<TType> _comparer;
        private readonly bool? _ignoreCase;

        private LazySafeResult(string elements, bool stopOnFailure = false, bool ignoreCase = true, params char[] separators)
            : this(elements, SafeResultType.HashSet, null, stopOnFailure, separators)
        {
            _ignoreCase = ignoreCase;
        }

        private LazySafeResult(string elements, SafeResultType type, bool stopOnFailure = false, params char[] separators) : this(elements, type, null, stopOnFailure, separators) { }

        private LazySafeResult(string elements, SafeResultType type, IEqualityComparer<TType> comparer, bool stopOnFailure = false, params char[] separators)
        {
            _elements = elements;
            _type = type;
            _comparer = comparer;
            _stopOnFailure = stopOnFailure;
            _separators = separators;
        }

        [BindNever]
        public T Result => SafeResult.Result;

        [BindNever]
        public bool HasFailure => SafeResult.HasFailure;

        [BindNever]
        public bool StopOnFailure => SafeResult.StopOnFailure;

        [BindNever]
        public int SuccessCount => SafeResult.SuccessCount;
        
        [BindNever]
        public int FailureCount => SafeResult.FailureCount;

        public static ISafeResult<List<TType>> List(string elements, bool stopOnFailure = false, params char[] separators)
        {
            return new LazySafeResult<List<TType>, TType>(elements, SafeResultType.List, stopOnFailure, separators);
        }

        public static ISafeResult<HashSet<TType>> EnumHashSet(string elements, bool stopOnFailure = false, bool ignoreCase = true, params char[] separators)
        {
            return new LazySafeResult<HashSet<TType>, TType>(elements, stopOnFailure, ignoreCase, separators);
        }

        public static ISafeResult<HashSet<TType>> HashSet(string elements, IEqualityComparer<TType> comparer, bool stopOnFailure = false, params char[] separators)
        {
            return new LazySafeResult<HashSet<TType>, TType>(elements, SafeResultType.HashSet, comparer, stopOnFailure, separators);
        }

        public static ISafeResult<HashSet<TType>> HashSet(string elements, bool stopOnFailure = false, params char[] separators)
        {
            return new LazySafeResult<HashSet<TType>, TType>(elements, SafeResultType.HashSet, stopOnFailure, separators);
        }

        public static ISafeResult<IDictionary<TType, int>> OrderedDictionary(string elements, IEqualityComparer<TType> comparer, bool stopOnFailure = false, params char[] separators)
        {
            return new LazySafeResult<IDictionary<TType, int>, TType>(elements, SafeResultType.OrderedDictionary, comparer, stopOnFailure, separators);
        }

        public static ISafeResult<IDictionary<TType, int>> OrderedDictionary(string elements, bool stopOnFailure = false, params char[] separators)
        {
            return new LazySafeResult<IDictionary<TType, int>, TType>(elements, SafeResultType.OrderedDictionary, stopOnFailure, separators);
        }

        private ISafeResult<T> _safeResult;

        private ISafeResult<T> SafeResult =>
            _safeResult ??
            (_safeResult = Get(_elements, _type, _comparer, _stopOnFailure, _ignoreCase, _separators));

        private static ISafeResult<T> Get(string elements, SafeResultType type, IEqualityComparer<TType> comparer, bool stopOnFailure, bool? ignoreCase, char[] separators)
        {
            switch (type)
            {
                case SafeResultType.HashSet:
                    return ignoreCase.HasValue
                        ? (ISafeResult<T>)SafeResult<TType>.EnumHashSet(elements, stopOnFailure, ignoreCase.Value, separators)
                        : comparer == null
                            ? (ISafeResult<T>)elements.ToSafeHashSet<TType>(stopOnFailure, separators)
                            : (ISafeResult<T>)elements.ToSafeHashSet(comparer, stopOnFailure);
                case SafeResultType.OrderedDictionary:
                    return comparer == null
                        ? (ISafeResult<T>)elements.ToSafeOrderedDictionary<TType>(stopOnFailure, separators)
                        : (ISafeResult<T>)elements.ToSafeOrderedDictionary(comparer, stopOnFailure, separators);
                case SafeResultType.List:
                default:
                    return (ISafeResult<T>)elements.ToSafeList<TType>(stopOnFailure, separators);
            }
        }
    }
}