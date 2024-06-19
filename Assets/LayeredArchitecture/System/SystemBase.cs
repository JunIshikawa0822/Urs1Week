public class SystemBase
{
    protected GameStatus gameStat;

    public void Init(GameStatus _gameStat)
    {
        this.gameStat = _gameStat;
    }

    public virtual void SetUp()
    {

    }
}
