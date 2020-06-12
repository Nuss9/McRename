namespace Renamer.Interfaces
{
    public interface IValidateComposeInstructions
    {
        (bool isValid, string errorMessage) Validate(ref ComposeInstructions instructions);
    }
}