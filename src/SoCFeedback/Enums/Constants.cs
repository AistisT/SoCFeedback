namespace SoCFeedback.Enums
{
    // Database needs to be updated via migrations if values here are changed, otherwise only client side values will change and database will still have old values
    public static class Constants
    {
        public const int ModuleTitleLength = 200;
        public const int UrlLength = 254;
        public const int AnswerLength = 3000;
        public const int ModuleDescLength = 3000;
        public const int ModuleCodeLength = 20;

        public const int CategoryTitleLength = 50;
        public const int CategoryDescriptionLength = 200;

        public const int NameLength = 50;
        public const int NameMinLength = 2;
        public const int EmailLength = 254;

        public const int LevelTitleLength = 50;
        public const int QuestionLength = 500;

        public const int PasswordMinLength = 14;
        public const int PasswordMaxLength = 100;
    }
}