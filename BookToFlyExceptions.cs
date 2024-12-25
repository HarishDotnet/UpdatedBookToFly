namespace BookToFlyExceptions
{    public class InvalidChoice : Exception
    {
        public InvalidChoice(string message):base(message){
            Console.Write("InvalidChoice : ");
        }
    }
    public class NullField : Exception
    {
        public NullField(string message):base(message){
            Console.Write("NullField Exception: ");
        }
    }
    public class SameAsUserName : Exception{
        public SameAsUserName(string message):base(message){
            Console.Write("SameAsUserName Exception: ");
        }
    }
     public class PasswordLength : Exception{
        public PasswordLength(string message):base(message){
            Console.Write("PasswordLength Exception : ");
        }
    }
    public class PasswordConditions : Exception{
        public PasswordConditions(string message):base(message){
            Console.Write("\nPassword shoud have the following: ");
        }
    }

}