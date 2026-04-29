using Application.Restaurants.Commands.CreateRestaurant;
using Application.Restaurants.Validators;
using FluentValidation.TestHelper;

namespace Application.Tests.Restaurants.commands.CreateRestaurant;

public class CreateRestaurantCommandValidatorTest
{
    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidatorErrors()
    {
        // arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Test",
            Category = "Italia",
            ContactEmail = "test@test.com",
            PostalCode = "12-345"
        };

        var validator = new CreateRestaurantCommandValidator();

        // act 

        var result = validator.TestValidate(command);

        // assert 
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ForInvalidCommand_ShouldHaveValidatorErrors()
    {
        // arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Te",
            Category = "Ita",
            ContactEmail = "test.com",
            PostalCode = "12345"
        };

        var validator = new CreateRestaurantCommandValidator();

        // act 

        var result = validator.TestValidate(command);

        // assert 
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Category);
        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

    [Theory]
    [InlineData("Italia")]
    [InlineData("Mexico")]
    [InlineData("Japanese")]
    [InlineData("American")]
    [InlineData("Indian")]
    public void Validator_ForValidCommand_ShouldNotHaveValidatorErrorsForCategoryProperty(string category)
    {
        // arrange
        var command = new CreateRestaurantCommand { Category = category };
        var validator = new CreateRestaurantCommandValidator();

        // act 

        var result = validator.TestValidate(command);

        // assert 
        result.ShouldNotHaveValidationErrorFor(c => c.Category);
    }

    [Theory]
    [InlineData("10220")]
    [InlineData("102-20")]
    [InlineData("10 220")]
    [InlineData("10-2 20")]
    public void Validator_ForInvalidCommand_ShouldHaveValidatorErrorsForPostalCodeProperty(string postalCode)
    {
        // arrange
        var command = new CreateRestaurantCommand { PostalCode = postalCode };
        var validator = new CreateRestaurantCommandValidator();

        // act 

        var result = validator.TestValidate(command);

        // assert 
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }
}