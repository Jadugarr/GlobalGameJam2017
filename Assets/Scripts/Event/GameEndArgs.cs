namespace Assets.Scripts.Event
{
    public class GameEndArgs : IEvent
    {
        public bool GameWon = false;

        public GameEndArgs(bool gameWon)
        {
            GameWon = gameWon;
        }
    }
}
