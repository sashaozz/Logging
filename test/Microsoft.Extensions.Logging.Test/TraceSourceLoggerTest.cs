// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET452
using System.Diagnostics;
using Xunit;

namespace Microsoft.Extensions.Logging.Test
{
    public class TraceSourceLoggerTest
    {
        [Fact]
        public static void IsEnabledReturnsCorrectValue()
        {
            // Arrange
            var testSwitch = new SourceSwitch("TestSwitch", "Level will be set to warning for this test");
            testSwitch.Level = SourceLevels.Warning;

            var factory = new LoggerFactory();
            var logger = factory.CreateLogger("Test");

            // Act
            factory.AddTraceSource(testSwitch);

            // Assert
            Assert.True(logger.IsEnabled(LogLevel.Critical));
            Assert.True(logger.IsEnabled(LogLevel.Error));
            Assert.True(logger.IsEnabled(LogLevel.Warning));
            Assert.False(logger.IsEnabled(LogLevel.Information));
            Assert.False(logger.IsEnabled(LogLevel.Debug));
            Assert.False(logger.IsEnabled(LogLevel.Trace));
        }

        [Theory]
        [InlineData(SourceLevels.Warning, SourceLevels.Information, true)]
        [InlineData(SourceLevels.Information, SourceLevels.Information, true)]
        [InlineData(SourceLevels.Information, SourceLevels.Warning, true)]
        [InlineData(SourceLevels.Warning, SourceLevels.Warning, false)]
        public static void MultipleLoggers_IsEnabledReturnsCorrectValue(SourceLevels first, SourceLevels second, bool expected)
        {
            // Arrange
            var firstSwitch = new SourceSwitch("FirstSwitch", "First Test Switch");
            firstSwitch.Level = first;

            var secondSwitch = new SourceSwitch("SecondSwitch", "Second Test Switch");
            secondSwitch.Level = second;

            var factory = new LoggerFactory();
            var logger = factory.CreateLogger("Test");

            // Act
            factory.AddTraceSource(firstSwitch);
            factory.AddTraceSource(secondSwitch);

            // Assert
            Assert.Equal(expected, logger.IsEnabled(LogLevel.Information));
        }
    }
}
#endif