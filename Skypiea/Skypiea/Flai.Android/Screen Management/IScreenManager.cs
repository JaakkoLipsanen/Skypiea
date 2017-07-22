
namespace Flai.ScreenManagement
{
    public interface IScreenManager
    {
        FlaiGame Game { get; }

        void AddScreen(GameScreen screen);
        void RemoveScreen(GameScreen screen);
    }
}
