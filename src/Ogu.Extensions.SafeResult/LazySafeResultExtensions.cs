using System.Collections.Generic;

namespace Ogu.Extensions.SafeResult
{
    public static class LazySafeResultExtensions
    {
        public static ISafeResult<List<TType>> ToLazySafeList<TType>(this string elements, bool stopOnFailure = false, params char[] separators)
        {
            return LazySafeResult<List<TType>, TType>.List(elements, stopOnFailure, separators);
        }

        public static ISafeResult<HashSet<TType>> ToLazySafeEnumHashSet<TType>(this string elements, bool stopOnFailure = false, bool ignoreCase = true, params char[] separators) where TType : struct, System.Enum
        {
            return LazySafeResult<HashSet<TType>, TType>.EnumHashSet(elements, stopOnFailure, ignoreCase, separators);
        }

        public static ISafeResult<HashSet<TType>> ToLazySafeHashSet<TType>(this string elements, IEqualityComparer<TType> comparer, bool stopOnFailure = false, params char[] separators)
        {
            return LazySafeResult<HashSet<TType>, TType>.HashSet(elements, comparer, stopOnFailure, separators);
        }

        public static ISafeResult<HashSet<TType>> ToLazySafeHashSet<TType>(this string elements, bool stopOnFailure = false, params char[] separators)
        {
            return LazySafeResult<HashSet<TType>, TType>.HashSet(elements, stopOnFailure, separators);
        }

        public static ISafeResult<IDictionary<TType, int>> ToLazySafeDictionary<TType>(this string elements, IEqualityComparer<TType> comparer, bool stopOnFailure = false, params char[] separators)
        {
            return LazySafeResult<IDictionary<TType, int>, TType>.OrderedDictionary(elements, comparer, stopOnFailure, separators);
        }

        public static ISafeResult<IDictionary<TType, int>> ToLazySafeDictionary<TType>(this string elements, bool stopOnFailure = false, params char[] separators)
        {
            return LazySafeResult<IDictionary<TType, int>, TType>.OrderedDictionary(elements, stopOnFailure, separators);
        }
    }
}