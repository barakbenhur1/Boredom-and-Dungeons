namespace BoredomAndDungeons
{
    public enum BDBossEncounterState
    {
        Dormant = 0,
        Intro = 1,
        Active = 2,
        Transition = 3,
        Victory = 4,
        Completed = 5,
        Failed = 6
    }

    public enum BDBossLifeState
    {
        Alive = 0,
        KnockedOut = 1,
        CriticalAtZero = 2,
        Dead = 3
    }
}
