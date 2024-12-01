using System;
using FluentAssertions;

namespace CSharpEssentials.Tests.Results;

public class UnitResultTests
{
    [Fact]
    public void SuccessResult_Should_Indicate_Success()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var isSuccess = result.IsSuccess;

        // Assert
        isSuccess.Should().BeTrue();
    }

    [Fact]
    public void FailureResult_Should_Indicate_Failure()
    {
        // Arrange
        Result result = Error.Failure();

        // Act
        var isFailure = result.IsFailure;

        // Assert
        isFailure.Should().BeTrue();
    }

    [Fact]
    public void FailureResult_Should_FirstErrorCode()
    {
        // Arrange
        Result result = Error.Failure("Error1", "Error 1 description");

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
        Result result = Error.Failure("Error1", "Error 1 description");

        // Act
        var lastError = result.LastError;

        // Assert
        lastError.Code.Should().Be("Error1");
        lastError.Description.Should().Be("Error 1 description");
    }

    [Fact]
    public void FailureResult_Should_MultipleErrors()
    {
        // Arrange
        Result result = Error.CreateMany(Error.Conflict(), Error.Forbidden());

        // Act
        var errorCount= result.Errors.Length;
        var firstError = result.FirstError;
        var lastError = result.LastError;

        // Assert
        errorCount.Should().Be(2);
        firstError.Code.Should().Be("Conflict");
        lastError.Code.Should().Be("Forbidden");
    }

    [Fact]
    public void SuccessResult_Should_FirstError()
    {
        // Arrange
        Result result = Result.Success();

        // Act
        var firstError = result.FirstError;

        // Assert
        firstError.Code.Should().Be("Result.NoFirstError");
    }

    [Fact]
    public void SuccessResult_Should_LastError()
    {
        // Arrange
        Result result = Result.Success();

        // Act
        var lastError = result.LastError;

        // Assert
        lastError.Code.Should().Be("Result.NoLastError");
    }

    [Fact]
    public void SuccessResult_Should_Errors_NoErrors(){
        // Arrange
        Result result = Result.Success();
        // Act
        var errors = result.Errors;

        // Assert
        errors.Length.Should().Be(1);
        errors[0].Code.Should().Be("Result.NoErrors");
    }

    [Fact]
    public void SuccessResult_Match_Success(){
        // Arrange
        Result result = Result.Success();
        // Act
        var isSuccess = result.Match(
            onSuccess: () => "success",
            onFailure: _ => "failure"
        );

        // Assert
        isSuccess.Should().Be("success");
    }

    [Fact]
    public void FailureResult_Match_Failure(){
        // Arrange
        Result result = Error.Failure("Error1", "Error 1 description");
        // Act
        var isSuccess = result.Match(
            onSuccess: () => "success",
            onFailure: errors => errors[0].Code
        );

        // Assert
        isSuccess.Should().Be("Error1");
    }

    [Fact]
    public void SuccessResult_MatchFirst_Success(){
        // Arrange
        Result result = Result.Success();
        // Act
        var isSuccess = result.MatchFirst(
            onSuccess: () => "success",
            onFirstError: _ => "failure"
        );

        // Assert
        isSuccess.Should().Be("success");
    } 

    [Fact]
    public void FailureResult_MatchFirst_Failure(){
        // Arrange
        Result result = Error.Failure("Error1", "Error 1 description");
        // Act
        var isSuccess = result.MatchFirst(
            onSuccess: () => "success",
            onFirstError: error => error.Code
        );

        // Assert
        isSuccess.Should().Be("Error1");
    }


    [Fact]
    public void SuccessResult_MatchLast_Success(){
        // Arrange
        Result result = Result.Success();
        // Act
        var isSuccess = result.MatchLast(
            onSuccess: () => "success",
            onLastError: _ => "failure"
        );

        // Assert
        isSuccess.Should().Be("success");
    }

    [Fact]
    public void FailureResult_MatchLast_Failure(){
        // Arrange
        Result result = Error.Failure("Error1", "Error 1 description");
        // Act
        var isSuccess = result.MatchLast(
            onSuccess: () => "success",
            onLastError: error => error.Code
        );

        // Assert
        isSuccess.Should().Be("Error1");
    }
}
