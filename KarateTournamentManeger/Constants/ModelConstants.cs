namespace KarateTournamentManager.Constants
{
    public static class ModelConstants
    {
        public const int MinLenght = 3;
        public const int MaxLenght = 50;
        public const int MixDescriptionLenght = 10;
        public const int MaxDescriptionLenght = 500;
        public const int MaxStageLenght = 50;
        public const string DateFormat = "dd-MM-yyyy";
        public const string TimerFormat = @"mm\:ss";
        public const int MaxTatamiLenght = 10;
        public const int MaxScoreLenght = 100;

        public static readonly Dictionary<string, string> StageToName = new Dictionary<string, string>
    {
        { "Preliminary", "Предварителен етап" },
        { "SixteenthFinal", "Шестнайсетинафинал" },
        { "EighthFinal", "Осминафинал" },
        { "QuarterFinal", "Четвъртфинал" },
        { "SemiFinal", "Полуфинал" },
        { "Final", "Финал" },
        { "RoundRobin", "Всеки срещу всеки" }
    };
    }
}
