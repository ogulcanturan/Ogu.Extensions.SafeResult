namespace Ogu.Extensions.SafeResult.Tests
{
    public class SafeResultExtensionsTests
    {
        [Fact]
        public void ToSafeList_GivenValidElements_ReturnsExpectedList()
        {
            // Arrange
            var elements = "1,2,3";
            var expected = new List<int> { 1, 2, 3 };

            // Act
            var safeResult = elements.ToSafeList<int>(false, ',');

            // Assert
            Assert.False(safeResult.IsThereAnyFailure);
            Assert.Equivalent(expected, safeResult.Result, strict: true);
            Assert.Equal(0, safeResult.FailureCount);
            Assert.Equal(3, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeList_GivenInvalidElements_ReturnsExpectedList()
        {
            // Arrange
            var elements = "1,2a, 3";
            var expected = new List<int> { 1, 3 };

            // Act
            var safeResult = elements.ToSafeList<int>(false, ',');

            // Assert
            Assert.True(safeResult.IsThereAnyFailure);
            Assert.Equivalent(expected, safeResult.Result, strict: true);
            Assert.Equal(1, safeResult.FailureCount);
            Assert.Equal(2, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeList_GivenInvalidElements_WithStopOnFailure_ReturnsExpectedList()
        {
            // Arrange
            var elements = "1,2a, 3";
            var expected = new List<int> { 1 };

            // Act
            var safeResult = elements.ToSafeList<int>(true, ',');

            // Assert
            Assert.True(safeResult.IsThereAnyFailure);
            Assert.Equivalent(expected, safeResult.Result, strict: true);
            Assert.Equal(1, safeResult.FailureCount);
            Assert.Equal(2, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeEnumHashSet_GivenValidElements_ReturnsExpectedHashSet()
        {
            // Arrange
            var elements = "monday,  tuesday,  wednesday";
            var expected = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday };

            // Act
            var safeResult = elements.ToSafeEnumHashSet<DayOfWeek>(false, true, ',');

            // Assert
            Assert.False(safeResult.IsThereAnyFailure);
            Assert.Equivalent(expected, safeResult.Result, strict: true);
            Assert.Equal(0, safeResult.FailureCount);
            Assert.Equal(3, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeEnumHashSet_GivenValidElementsNumber_ReturnsExpectedHashSet()
        {
            // Arrange
            var elements = " 1,2,3";
            var expected = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday };

            // Act
            var safeResult = elements.ToSafeEnumHashSet<DayOfWeek>(false, true, ',');

            // Assert
            Assert.False(safeResult.IsThereAnyFailure);
            Assert.Equivalent(expected, safeResult.Result, strict: true);
            Assert.Equal(0, safeResult.FailureCount);
            Assert.Equal(3, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeEnumHashSet_GivenValidElements_WithIgnoreCaseFalse_ReturnsExpectedHashSet()
        {
            // Arrange
            var elements = "monday,Tuesday,wednesday";
            var expected = new HashSet<DayOfWeek> { DayOfWeek.Tuesday };

            // Act
            var safeResult = elements.ToSafeEnumHashSet<DayOfWeek>(false, false, ',');

            // Assert
            Assert.True(safeResult.IsThereAnyFailure);
            Assert.Equivalent(expected, safeResult.Result, strict: true);
            Assert.Equal(2, safeResult.FailureCount);
            Assert.Equal(1, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeEnumHashSet_GivenInvalidElements_ReturnsExpectedHashSet()
        {
            // Arrange
            var elements = "Monday,Twuesday,Wednesday";
            var expected = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday };

            // Act
            var safeResult = elements.ToSafeEnumHashSet<DayOfWeek>(false, true, ',');

            // Assert
            Assert.True(safeResult.IsThereAnyFailure);
            Assert.Equivalent(expected, safeResult.Result, strict: true);
            Assert.Equal(1, safeResult.FailureCount);
            Assert.Equal(2, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeEnumHashSet_GivenInvalidElements_WithStopOnFailure_ReturnsExpectedHashSet()
        {
            // Arrange
            var elements = "monday,Twuesday,Mwednesday";
            var expected = new HashSet<DayOfWeek> { DayOfWeek.Monday  };

            // Act
            var safeResult = elements.ToSafeEnumHashSet<DayOfWeek>(true, true, ',');

            // Assert
            Assert.True(safeResult.IsThereAnyFailure);
            Assert.Equivalent(expected, safeResult.Result, strict: true);
            Assert.Equal(1, safeResult.FailureCount);
            Assert.Equal(1, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeHashSet_GivenValidElements_WithComparer_ReturnsExpectedHashSet()
        {
            // Arrange
            var elements = "a,b,c";
            var comparer = StringComparer.OrdinalIgnoreCase;
            var expected = new HashSet<string>(new[] { "a", "b", "c" }, comparer);

            // Act
            var safeResult = elements.ToSafeHashSet(comparer, false, ',');

            // Assert
            Assert.False(safeResult.IsThereAnyFailure);
            Assert.Equivalent(expected, safeResult.Result, strict: true);
            Assert.Equal(0, safeResult.FailureCount);
            Assert.Equal(3, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeHashSet_GivenValidElements_WithoutComparer_ReturnsExpectedHashSet()
        {
            // Arrange
            var elements = "1,2,3";
            var expected = new HashSet<int> { 1, 2, 3 };

            // Act
            var safeResult = elements.ToSafeHashSet<int>(false, ',');

            // Assert
            Assert.False(safeResult.IsThereAnyFailure);
            Assert.Equivalent(expected, safeResult.Result, strict: true);
            Assert.Equal(0, safeResult.FailureCount);
            Assert.Equal(3, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeHashSet_GivenInvalidElementsNumber_ReturnsExpectedHashSet()
        {
            // Arrange
            var elements = "1a,2,3a";
            var expected = new HashSet<int> { 2 };

            // Act
            var safeResult = elements.ToSafeHashSet<int>(false, ',');

            // Assert
            Assert.True(safeResult.IsThereAnyFailure);
            Assert.Equivalent(expected, safeResult.Result, strict: true);
            Assert.Equal(2, safeResult.FailureCount);
            Assert.Equal(1, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeHashSet_GivenInvalidElementsNumber_WithStopOnFailure_ReturnsExpectedHashSet()
        {
            // Arrange
            var elements = "1a,2,3a";
            var expected = new HashSet<int> { };

            // Act
            var safeResult = elements.ToSafeHashSet<int>(true, ',');

            // Assert
            Assert.True(safeResult.IsThereAnyFailure);
            Assert.Equivalent(expected, safeResult.Result, strict: true);
            Assert.Equal(1, safeResult.FailureCount);
            Assert.Equal(0, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeOrderedDictionary_GivenValidElements_WithComparer_ReturnsExpectedDictionary()
        {
            // Arrange
            var elements = "a,b,c";
            var comparer = StringComparer.OrdinalIgnoreCase;
            var expected = new Dictionary<string, int>(comparer)
            {
                { "a", 0 },
                { "b", 1 },
                { "c", 2 }
            };

            // Act
            var safeResult = elements.ToSafeOrderedDictionary(comparer, false, ',');

            // Assert
            Assert.False(safeResult.IsThereAnyFailure);
            Assert.Equivalent(expected, safeResult.Result, strict: true);
            Assert.Equal(0, safeResult.FailureCount);
            Assert.Equal(3, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeOrderedDictionary_GivenValidElements_WithoutComparer_ReturnsExpectedDictionary()
        {
            // Arrange
            var elements = "1,2,3";
            var expected = new Dictionary<int, int>
            {
                { 1, 0 },
                { 2, 1 },
                { 3, 2 }
            };

            // Act
            var safeResult = elements.ToSafeOrderedDictionary<int>(false, ',');

            // Assert
            Assert.False(safeResult.IsThereAnyFailure);
            Assert.Equal(expected, safeResult.Result);
            Assert.Equal(0, safeResult.FailureCount);
            Assert.Equal(3, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeOrderedDictionary_GivenInvalidElements_WithoutComparer_ReturnsExpectedDictionary()
        {
            // Arrange
            var elements = "1 a, 2 ,3";
            var expected = new Dictionary<int, int>
            {
                { 2, 0 },
                { 3, 1 }
            };

            // Act
            var safeResult = elements.ToSafeOrderedDictionary<int>(false, ',');

            // Assert
            Assert.True(safeResult.IsThereAnyFailure);
            Assert.Equal(expected, safeResult.Result);
            Assert.Equal(1, safeResult.FailureCount);
            Assert.Equal(2, safeResult.SuccessCount);
        }

        [Fact]
        public void ToSafeOrderedDictionary_GivenInvalidElements_WithStopOnFailure_ReturnsExpectedDictionary()
        {
            // Arrange
            var elements = "1, 2 a ,3";
            var expected = new Dictionary<int, int>
            {
                { 1, 0 }
            };

            // Act
            var safeResult = elements.ToSafeOrderedDictionary<int>(true, ',');

            // Assert
            Assert.True(safeResult.IsThereAnyFailure);
            Assert.Equal(expected, safeResult.Result);
            Assert.Equal(1, safeResult.FailureCount);
            Assert.Equal(1, safeResult.SuccessCount);
        }
    }
}