namespace Journey.UI
{
    public class Class1
    {
        public int result(string version1, string version2)
        {
            string paddedInput1 = "";
            string paddedInput2 = "";

            if (version1.Length == version2.Length && version1 == version2)
                return 0;

            //parts 
            var input1Parts = version1.Split('.');
            var input2Parts = version2.Split('.');

            var lengthDifference = Math.Abs(input1Parts.Length - input2Parts.Length);

            Func<string, string> EnsureLengthIsSame = (targetInput) =>
            {
                string cleanedInput = targetInput;
                while (lengthDifference > 0)
                {
                    cleanedInput += ".0";

                    lengthDifference--;
                }

                return cleanedInput;
            };

            if (input1Parts.Length > input2Parts.Length)
            {
                paddedInput1 = EnsureLengthIsSame(version2);
            } 
            else
            {
                paddedInput1 = EnsureLengthIsSame(version1);
            }
                

            return string.Compare(paddedInput1, paddedInput2);
          
        }

    }
}