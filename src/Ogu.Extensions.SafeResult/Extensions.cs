using System.Collections.Generic;

namespace Ogu.Extensions.SafeResult
{
    public static class Extensions
    {
        public static ISafeResult<List<TType>> ToSafeList<TType>(this string elements, bool stopOnFailure = false, params char[] separators)
        {
            return SafeResult<TType>.List(elements, stopOnFailure, separators);
        }

        public static ISafeResult<HashSet<TType>> ToSafeHashSet<TType>(this string elements, bool stopOnFailure = false, bool ignoreCase = true, params char[] separators) where TType : struct, System.Enum
        {
            return SafeResult<TType>.EnumHashSet(elements, stopOnFailure, ignoreCase, separators);
        }

        public static ISafeResult<HashSet<TType>> ToSafeHashSet<TType>(this string elements, IEqualityComparer<TType> comparer, bool stopOnFailure = false, params char[] separators)
        {
            return SafeResult<TType>.HashSet(elements, comparer, stopOnFailure, separators);
        }

        public static ISafeResult<HashSet<TType>> ToSafeHashSet<TType>(this string elements, bool stopOnFailure = false, params char[] separators)
        {
            return SafeResult<TType>.HashSet(elements, stopOnFailure, separators);
        }

        public static ISafeResult<IDictionary<TType, int>> ToSafeDictionary<TType>(this string elements, IEqualityComparer<TType> comparer, bool stopOnFailure = false, params char[] separators)
        {
            return SafeResult<TType>.Dictionary(elements, comparer, stopOnFailure, separators);
        }

        public static ISafeResult<IDictionary<TType, int>> ToSafeDictionary<TType>(this string elements, bool stopOnFailure = false, params char[] separators)
        {
            return SafeResult<TType>.Dictionary(elements, stopOnFailure, separators);
        }
    }
}