using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Theta.XSPOC.Apex.Api.Common.Tests
{
    [TestClass]
    public class PasswordUtilityTests
    {
        private const string USERNAME = "theta";
        private const string PASSWORD = "Pa$$word";
        private const string EXPECTED_HASH = @"2hh2yHUoHE3QXGx8otPdNbPb3/IdBbkR2JHdshd1vc3sjWRM";

        [TestMethod]
        public void HashPasswordTest()
        {
            string hash;
            try
            {
                hash = PasswordUtility.HashPassword(null, "password");

                Assert.Fail("An exception was not thrown when username was null.");
            }
            catch (ArgumentNullException) { }

            try
            {
                hash = PasswordUtility.HashPassword("username", null);

                Assert.Fail("An exception was not thrown when password was null.");
            }
            catch (ArgumentNullException) { }

            hash = PasswordUtility.HashPassword(USERNAME, PASSWORD);

            Assert.AreEqual(EXPECTED_HASH, hash, "The hash was not as expected.");
        }

        [TestMethod]
        public void VerifyPasswordTest()
        {
            bool success;
            try
            {
                success = PasswordUtility.VerifyPassword(null, "password", "hash");

                Assert.Fail("An exception was not thrown when username was null.");
            }
            catch (ArgumentNullException) { }

            try
            {
                success = PasswordUtility.VerifyPassword("username", null, "hash");

                Assert.Fail("An exception was not thrown when password was null.");
            }
            catch (ArgumentNullException) { }

            try
            {
                success = PasswordUtility.VerifyPassword("username", "password", null);

                Assert.Fail("An exception was not thrown when hash was null.");
            }
            catch (ArgumentNullException) { }

            success = PasswordUtility.VerifyPassword(USERNAME, PASSWORD, EXPECTED_HASH);

            Assert.IsTrue(success, "Password verification failed unexpectedly.");

            success = PasswordUtility.VerifyPassword(USERNAME, "WrongPassword", EXPECTED_HASH);

            Assert.IsFalse(success, "Password verification passed unexpectedly.");
        }

#if false
        [TestMethod]
        //This was used to generate the value for SALT_KEY in PasswordUtility
        //I left the code here in case we ever want to generate a new one
        //Source: http://msdn.microsoft.com/en-us/library/aa545602.aspx
        public void GenerateSaltKey()
        {
            const int SALT_SIZE = 16;

            // Create a random number object seeded from the value
            // of the last random seed value. This is done
            // interlocked because it is a static value and we want
            // it to roll forward safely.
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));

            // Create an array of random values.
            byte[] saltValue = new byte[SALT_SIZE];

            random.NextBytes(saltValue);

            // Return the salt value as a string.
            Console.WriteLine(Convert.ToBase64String(saltValue));
        }
#endif
    }
}
