﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Chess.Common.Tests
{
    public class SquareReferenceTests
    {
        [Fact]
        public void CastingValidIntArray_ReturnsReference()
        {
            int[] input = new[] { 1, 3 };

            var reference = (SquareReference)input;

            Assert.Equal(1, reference.Row);
            Assert.Equal(3, reference.Column);
        }

        [Fact]
        public void CastingIntArrayWithTooFewElements_ThrowsArgumentException()
        {
            int[] input = new[] { 1 };

            Assert.Throws<ArgumentException>(() =>
            {
                var reference = (SquareReference)input;
            });
        }
        [Fact]
        public void CastingIntArrayWithManyFewElements_ThrowsArgumentException()
        {
            int[] input = new[] { 1,2,3 };

            Assert.Throws<ArgumentException>(() =>
            {
                var reference = (SquareReference)input;
            });
        }

        [Fact]
        public void CastingIntArrayWithTooLowRow_ThrowsArgumentException()
        {
            int[] input = new[] { -1, 3 };

            Assert.Throws<ArgumentException>(() =>
            {
                var reference = (SquareReference)input;
            });
        }
        [Fact]
        public void CastingIntArrayWithTooHighRow_ThrowsArgumentException()
        {
            int[] input = new[] { 8, 3 };

            Assert.Throws<ArgumentException>(() =>
            {
                var reference = (SquareReference)input;
            });
        }
        [Fact]
        public void CastingIntArrayWithTooLowColumn_ThrowsArgumentException()
        {
            int[] input = new[] { 3, -1 };

            Assert.Throws<ArgumentException>(() =>
            {
                var reference = (SquareReference)input;
            });
        }
        [Fact]
        public void CastingIntArrayWithTooHighColumn_ThrowsArgumentException()
        {
            int[] input = new[] { 3, 8 };

            Assert.Throws<ArgumentException>(() =>
            {
                var reference = (SquareReference)input;
            });
        }


        [Fact]
        public void CastingValidString_ReturnsReference()
        {
            var reference = (SquareReference)"b3";

            Assert.Equal(2, reference.Row);
            Assert.Equal(1, reference.Column);
        }
        [Fact]
        public void CastingTooShortString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var reference = (SquareReference)"b";
            });
        }
        [Fact]
        public void CastingTooLongString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var reference = (SquareReference)"basd";
            });
        }
        [Fact]
        public void CastingOutOfRangeColumn_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var reference = (SquareReference)"t3";
            });
        }
        [Fact]
        public void CastingOutOfRangeRow_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var reference = (SquareReference)"b9";
            });
        }



        [Fact]
        public void Equality_WhenSameValues_ReturnsTrue()
        {
            var x = SquareReference.FromRowColumn(1, 3);
            var y = SquareReference.FromRowColumn(1, 3);

            Assert.True(x == y);
        }
        [Fact]
        public void Equality_WhenRowDiffers_ReturnsFalse()
        {
            var x = SquareReference.FromRowColumn(1, 3);
            var y = SquareReference.FromRowColumn(2, 3);

            Assert.False(x == y);
        }
        [Fact]
        public void Equality_WhenColumnDiffers_ReturnsFalse()
        {
            var x = SquareReference.FromRowColumn(1, 3);
            var y = SquareReference.FromRowColumn(1, 4);

            Assert.False(x == y);
        }


        [Fact]
        public void Inequality_WhenSameValues_ReturnsTrue()
        {
            var x = SquareReference.FromRowColumn(1, 3);
            var y = SquareReference.FromRowColumn(1, 3);

            Assert.False(x != y);
        }
        [Fact]
        public void Inequality_WhenRowDiffers_ReturnsFalse()
        {
            var x = SquareReference.FromRowColumn(1, 3);
            var y = SquareReference.FromRowColumn(2, 3);

            Assert.True(x != y);
        }
        [Fact]
        public void Inequality_WhenColumnDiffers_ReturnsFalse()
        {
            var x = SquareReference.FromRowColumn(1, 3);
            var y = SquareReference.FromRowColumn(1, 4);

            Assert.True(x != y);
        }
    }
}
