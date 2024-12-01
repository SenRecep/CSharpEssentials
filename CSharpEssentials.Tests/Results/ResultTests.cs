using System;
using FluentAssertions;

namespace CSharpEssentials.Tests.Results;

public class ResultTests
{
    [Fact]
    public void SuccessResult_Should_Indicate_Success()
    {
        // Arrange
        Result<int> result = 1;

        // Act
        var isSuccess = result.IsSuccess;

        // Assert
        isSuccess.Should().BeTrue();
    }

    [Fact]
    public void FailureResult_Should_Indicate_Failure()
    {
        // Arrange
        Result<int> result = Error.Failure("Error1", "Error 1 description");

        // Act
        var isFailure = result.IsFailure;

        // Assert
        isFailure.Should().BeTrue();
    }

    [Fact]
    public void FailureResult_Should_FirstErrorCode()
    {
        // Arrange
        Result<int> result = Error.Failure("Error1", "Error 1 description");

        // Act
        var firstError = result.FirstError;

        // Assert
        firstError.Code.Should().Be("Error1");
        firstError.Description.Should().Be("Error 1 description");
    }

    [Fact]
    public void FailureResult_Should_LastErrorCode()
    {
        // Arrange
        Result<int> result = Error.Failure("Error1", "Error 1 description");

        // Act
        var lastError = result.LastError;

        // Assert
        lastError.Code.Should().Be("Error1");
        lastError.Description.Should().Be("Error 1 description");
    }

    [Fact]
    public void SuccessResult_Should_Indicate_Success_With_Value()
    {
        // Arrange
        Result<int> result = 1;

        // Act
        var isSuccess = result.IsSuccess;

        // Assert
        isSuccess.Should().BeTrue();
        result.Value.Should().Be(1);
    }

    [Fact]
    public void FailureResult_Should_Indicate_Failure_With_Value()
    {
        // Arrange
        Result<int> result = Error.Failure("Error1", "Error 1 description");

        // Act
        var isFailure = result.IsFailure;

        // Assert
        isFailure.Should().BeTrue();
        result.Value.Should().Be(default);
    }

    [Fact]
    public void FailureResult_Should_FirstErrorCode_With_Value()
    {
        // Arrange
        Result<int> result = Error.Failure("Error1", "Error 1 description");

        // Act
        var firstError = result.FirstError;

        // Assert
        firstError.Code.Should().Be("Error1");
        firstError.Description.Should().Be("Error 1 description");
        result.Value.Should().Be(default);
    }

    [Fact]
    public void FailureResult_Should_LastErrorCode_With_Value()
    {
        // Arrange
        Result<int> result = Error.Failure("Error1", "Error 1 description");

        // Act
        var lastError = result.LastError;

        // Assert
        lastError.Code.Should().Be("Error1");
        lastError.Description.Should().Be("Error 1 description");
        result.Value.Should().Be(default);
    }

    [Fact]
    public void FailureResult_Should_MultipleErrors()
    {
        // Arrange
        Result<int> result = Error.CreateMany(Error.Conflict(), Error.Forbidden());

        // Act
        var errors = result.Errors;

        // Assert
        errors.Should().HaveCount(2);
        errors[0].Code.Should().Be("Conflict");
        errors[0].Description.Should().Be("A conflict error has occurred.");
        errors[1].Code.Should().Be("Forbidden");
        errors[1].Description.Should().Be("A 'Forbidden' error has occurred.");
    }
}
