namespace Utils.BookToFlyExceptions
{
    // Exception for invalid choices entered by the user
    public class InvalidChoice : Exception
    {
        // Constructor to initialize the exception with a custom message
        public InvalidChoice(string message) : base(message)
        {
            Console.Write("InvalidChoice : ");
        }
    }

    // Exception for when a required field is null or empty
    public class NullField : Exception
    {
        // Constructor to initialize the exception with a custom message
        public NullField(string message) : base(message)
        {
            Console.Write("NullField Exception: ");
        }
    }

    // Exception for when the username entered is the same as an existing one
    public class SameAsUserName : Exception
    {
        // Constructor to initialize the exception with a custom message
        public SameAsUserName(string message) : base(message)
        {
            Console.Write("SameAsUserName Exception: ");
        }
    }

    // Exception for password length being shorter or longer than the allowed limit
    public class PasswordLength : Exception
    {
        // Constructor to initialize the exception with a custom message
        public PasswordLength(string message) : base(message)
        {
            Console.Write("PasswordLength Exception : ");
        }
    }

    // Exception for when the password does not meet the required conditions (e.g., special characters, uppercase, etc.)
    public class PasswordConditions : Exception
    {
        // Constructor to initialize the exception with a custom message
        public PasswordConditions(string message) : base(message)
        {
            Console.Write("\nPassword should have the following: ");
        }
    }
}
