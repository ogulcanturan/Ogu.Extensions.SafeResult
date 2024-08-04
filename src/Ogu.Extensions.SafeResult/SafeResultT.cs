using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ogu.Extensions.SafeResult
{
    public class SafeResult<TType> : ISafeResult<TType>
    {
        private SafeResult(TType result, bool isThereAnyFailure, bool stopOnFailure, int successCount, int failureCount)
        {
            Result = result;
            HasFailure = isThereAnyFailure;
            StopOnFailure = stopOnFailure;
            SuccessCount = successCount;
            FailureCount = failureCount;
        }

        [BindNever]
        public TType Result { get; }

        [BindNever]
        public bool HasFailure { get; }

        [BindNever]
        public bool StopOnFailure { get; }

        [BindNever]
        public int SuccessCount { get; }

        [BindNever]
        public int FailureCount { get; }

        public static ISafeResult<List<TType>> List(string elements, bool stopOnFailure = false, params char[] separators)
        {
            var result = new List<TType>();

            if (string.IsNullOrWhiteSpace(elements))
            {
                return new SafeResult<List<TType>>(result, isThereAnyFailure: false, stopOnFailure, successCount: 0, failureCount: 0);
            }

            var isThereAnyFailure = false;
            int successCount = 0, failureCount = 0;
            var type = typeof(TType);

            var items = elements.Split(separators?.Length > 0 ? separators : Constants.CommaSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()).Where(s => s != string.Empty);

            foreach (var item in items)
            {
                try
                {
                    var parsedItem = (TType)Convert.ChangeType(item, type);
                    result.Add(parsedItem);
                    successCount++;
                }
                catch
                {
                    isThereAnyFailure = true;
                    failureCount++;

                    if (stopOnFailure)
                    {
                        break;
                    }
                }
            }

            return new SafeResult<List<TType>>(result, isThereAnyFailure, stopOnFailure, successCount, failureCount);
        }

        public static ISafeResult<HashSet<TType>> EnumHashSet(string elements, bool stopOnFailure = false, bool ignoreCase = true, params char[] separators)
        {
            var result = new HashSet<TType>();

            if (string.IsNullOrWhiteSpace(elements))
            {
                return new SafeResult<HashSet<TType>>(result, isThereAnyFailure: false, stopOnFailure, successCount: 0, failureCount: 0);
            }

            var isThereAnyFailure = false;
            int successCount = 0, failureCount = 0;
            var type = typeof(TType);

            var items = elements.Split(separators?.Length > 0 ? separators : Constants.CommaSeparator, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Where(s => s != string.Empty);

            foreach (var item in items)
            {
                if (
#if NETSTANDARD2_0
                    EnumTryParse(type, item, ignoreCase, out var enumResult)
#else
                    Enum.TryParse(type, item, ignoreCase, out var enumResult)
#endif
                    && Enum.IsDefined(type, enumResult)
                )
                {
                    result.Add((TType)enumResult);
                    successCount++;
                }
                else
                {
                    isThereAnyFailure = true;
                    failureCount++;

                    if (stopOnFailure)
                    {
                        break;
                    }
                }
            }

            return new SafeResult<HashSet<TType>>(result, isThereAnyFailure, stopOnFailure, successCount, failureCount);
        }

        public static ISafeResult<HashSet<TType>> HashSet(string elements, IEqualityComparer<TType> comparer, bool stopOnFailure = false, params char[] separators)
        {
            return InternalHashSet(elements, comparer, stopOnFailure, separators);
        }

        public static ISafeResult<HashSet<TType>> HashSet(string elements, bool stopOnFailure = false, params char[] separators)
        {
            return InternalHashSet(elements, EqualityComparer<TType>.Default, stopOnFailure, separators);
        }

        public static ISafeResult<IDictionary<TType, int>> OrderedDictionary(string elements, IEqualityComparer<TType> comparer, bool stopOnFailure = false, params char[] separators)
        {
            return InternalOrderedDictionary(new Dictionary<TType, int>(comparer), elements, stopOnFailure, separators);
        }

        public static ISafeResult<IDictionary<TType, int>> OrderedDictionary(string elements, bool stopOnFailure = false, params char[] separators)
        {
            return InternalOrderedDictionary(new Dictionary<TType, int>(), elements, stopOnFailure, separators);
        }

        private static ISafeResult<HashSet<TType>> InternalHashSet(string elements, IEqualityComparer<TType> comparer, bool stopOnFailure, params char[] separators)
        {
            var result = new HashSet<TType>(comparer);
            
            if (string.IsNullOrWhiteSpace(elements))
            {
                return new SafeResult<HashSet<TType>>(result, isThereAnyFailure: false, stopOnFailure, successCount: 0, failureCount: 0);
            }

            var isThereAnyFailure = false;
            int successCount = 0, failureCount = 0;
            var type = typeof(TType);

            var items = elements.Split(separators?.Length > 0 ? separators : Constants.CommaSeparator, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Where(s => s != string.Empty);

            foreach (var item in items)
            {
                try
                {
                    var parsedItem = (TType)Convert.ChangeType(item, type);
                    result.Add(parsedItem);
                    successCount++;
                }
                catch
                {
                    isThereAnyFailure = true;
                    failureCount++;

                    if (stopOnFailure)
                    {
                        break;
                    }
                }
            }

            return new SafeResult<HashSet<TType>>(result, isThereAnyFailure, stopOnFailure, successCount, failureCount);
        }

        private static ISafeResult<IDictionary<TType, int>> InternalOrderedDictionary(IDictionary<TType, int> dictionary, string elements, bool stopOnFailure, params char[] separators)
        {
            if (string.IsNullOrWhiteSpace(elements))
            {
                return new SafeResult<IDictionary<TType, int>>(dictionary, isThereAnyFailure: false, stopOnFailure, successCount: 0, failureCount: 0);
            }

            var isThereAnyFailure = false;
            int successCount = 0, failureCount = 0;
            var type = typeof(TType);
            var index = 0;

            var items = elements.Split(separators?.Length > 0 ? separators : Constants.CommaSeparator, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim()).Where(item => item != string.Empty);

            foreach (var item in items)
            {
                try
                {
                    var convertedItem = (TType)Convert.ChangeType(item, type);

                    dictionary.Add(convertedItem, index);
                    index++;
                    successCount++;
                }
                catch
                {
                    isThereAnyFailure = true;
                    failureCount++;

                    if (stopOnFailure)
                    {
                        break;
                    }
                }
            }

            return new SafeResult<IDictionary<TType, int>>(dictionary, isThereAnyFailure, stopOnFailure, successCount, failureCount);
        }

#if NETSTANDARD2_0
        private static bool EnumTryParse(Type type, string value, bool ignoreCase, out object result)
        {
            try
            {
                result = Enum.Parse(type, value, ignoreCase);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
#endif
    }
}