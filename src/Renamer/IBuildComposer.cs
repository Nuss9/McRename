namespace Renamer
{
    public interface IBuildComposer
    {
        ICompose Build(ComposeMode mode);
    }
}
