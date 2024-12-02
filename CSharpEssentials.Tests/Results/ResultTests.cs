using System;
using FluentAssertions;

namespace CSharpEssentials.Tests.Results;

public class ResultTests
{
    private record Person(string Name);

    [Fact]
    public void CreateFromFactory_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        IEnumerable<string> value = ["value"];

        // Act
        Result<IEnumerable<string>> result = Result<IEnumerable<string>>.Success(value);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().BeSameAs(value);
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingErrors_ShouldThrow()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        Result<IEnumerable<string>> result = Result<IEnumerable<string>>.Success(value);

        // Act
        Func<Error[]> errors = () => result.Errors;

        // Assert
        errors().Should().HaveCount(1);
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        Result<IEnumerable<string>> result = Result<IEnumerable<string>>.Success(value);

        // Act
        Error[] errors = result.ErrorsOrEmptyArray;

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingFirstError_ShouldThrow()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        Result<IEnumerable<string>> result = Result<IEnumerable<string>>.Success(value);

        // Act
        Func<Error> action = () => result.FirstError;

        // Assert
        action().Code.Should().Be("Result.NoFirstError");
    }

    [Fact]
    public void CreateFromValue_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        IEnumerable<string> value = ["value"];

        // Act
        Result<IEnumerable<string>> result = Result<IEnumerable<string>>.Success(value);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().BeSameAs(value);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingErrors_ShouldThrow()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        Result<IEnumerable<string>> result = Result<IEnumerable<string>>.Success(value);

        // Act
        Func<Error[]> action = () => result.Errors;

        // Assert
        action().Should().HaveCount(1);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        Result<IEnumerable<string>> result = Result<IEnumerable<string>>.Success(value);

        // Act
        Error[] errors = result.ErrorsOrEmptyArray;

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void CreateFromValue_WhenAccessingFirstError_ShouldThrow()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        Result<IEnumerable<string>> result = Result<IEnumerable<string>>.Success(value);

        // Act
        Func<Error> action = () => result.FirstError;

        // Assert
        action().Code.Should().Be("Result.NoFirstError");
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        Error[] errors = [Error.Validation("User.Name", "Name is too short")];
        Result<Person> result = Result<Person>.From(errors);

        // Act & Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(errors.Single());
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingErrorsOrEmptyList_ShouldReturnErrorList()
    {
        // Arrange
        Error[] errors = [Error.Validation("User.Name", "Name is too short")];
        Result<Person> result = Result<Person>.From(errors);

        // Act & Assert
        result.IsFailure.Should().BeTrue();
        result.ErrorsOrEmptyArray.Should().ContainSingle().Which.Should().Be(errors.Single());
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingValue_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Error[] errors = [Error.Validation("User.Name", "Name is too short")];
        Result<Person> result = Result<Person>.From(errors);

        // Act
        var act = () => result.Value;

        // Assert
        act().Should().Be(default(Person));
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingResult_ShouldReturnValue()
    {
        // Arrange
        Person result = new Person("Amici");

        // Act
        Result<Person> errorOr = result;

        // Assert
        errorOr.IsFailure.Should().BeFalse();
        errorOr.Value.Should().Be(result);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingErrors_ShouldThrow()
    {
        Result<Person> result = new Person("Amichai");

        // Act
        Func<Error[]> action = () => result.Errors;

        // Assert
        action().Should().HaveCount(1);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingFirstError_ShouldThrow()
    {
        Result<Person> result = new Person("Amichai");

        // Act
        Func<Error> action = () => result.FirstError;

        // Assert
        action().Code.Should().Be("Result.NoFirstError");
    }

    [Fact]
    public void ImplicitCastPrimitiveResult_WhenAccessingResult_ShouldReturnValue()
    {
        // Arrange
        const int value = 4;

        // Act
        Result<int> result = value;

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        Error error = Error.Validation("User.Name", "Name is too short");

        // Act
        Result<Person> result = error;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void ImplicitCastError_WhenAccessingValue_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Result<Person> result = Error.Validation("User.Name", "Name is too short");

        // Act
        var act = () => result.Value;

        // Assert
        act().Should().BeNull();
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingFirstError_ShouldReturnError()
    {
        // Arrange
        Error error = Error.Validation("User.Name", "Name is too short");

        // Act
        Result<Person> result = error;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError.Should().Be(error);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        Error[] errors = [

            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        Result<Person> result = errors;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(errors.Length).And.BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingErrors_ShouldReturnErrorArray()
    {
        // Arrange
        Error[] errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        Result<Person> result = errors;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(errors.Length).And.BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        // Arrange
        Error[] errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        Result<Person> result = errors;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError.Should().Be(errors[0]);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        // Arrange
        Error[] errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        Result<Person> result = errors;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError.Should().Be(errors[0]);
    }

    [Fact]
    public void CreateErrorOr_WhenUsingEmptyConstructor_ShouldThrow()
    {
        // Act
        Func<Result<int>> action = () => new Result<int>();

        // Assert
        action.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void CreateErrorOr_WhenEmptyErrorsArray_ShouldThrow()
    {
        // Act
        Func<Result<int>> result = () => Array.Empty<Error>();

        // Assert
        var exception = result.Should().ThrowExactly<ArgumentException>().Which;
        exception.Message.Should().Be("Cannot create an Result from an empty collection of errors. Provide at least one error.");
    }

    [Fact]
    public void CreateErrorOr_WhenNullIsPassedAsErrorsList_ShouldThrowArgumentNullException()
    {
        Func<Result<int>> act = () => default(Error[])!;

        act.Should().ThrowExactly<ArgumentNullException>()
           .And.ParamName.Should().Be("errors");
    }

    [Fact]
    public void CreateErrorOr_WhenNullIsPassedAsErrorsArray_ShouldThrowArgumentNullException()
    {
        Func<Result<int>> act = () => default(Error[])!;

        act.Should().ThrowExactly<ArgumentNullException>()
           .And.ParamName.Should().Be("errors");
    }

    [Fact]
    public void CreateErrorOr_WhenValueIsNull_ShouldThrowArgumentNullException()
    {
        Func<Result<int?>> act = () => default(int?);

        act.Should().ThrowExactly<ArgumentNullException>()
           .And.ParamName.Should().Be("value");
    }
}
