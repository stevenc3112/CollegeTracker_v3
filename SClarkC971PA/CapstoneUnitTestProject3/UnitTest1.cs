using System;
using Xunit;
namespace CapstoneUnitTestProject3
{
    public class UnitTest1
    {
        [Fact]
        public void ValidateDates_StartBeforeEnd_ReturnsTrue()
        {
            
           bool result = Services.ValidateDates(
                new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1));

            Assert.True(result);
        }

        [Fact]
        public void ValidateDates_StartAfterEnd_ReturnsFalse()
        {
            bool result = Services.ValidateDates(
                 new DateTime(2025, 2, 1),
                 new DateTime(2025, 1, 1));

            Assert.False(result);
        }
        [Fact]
        public void ValidatePassword_PasswordWasHashed()
        {
            bool result = false;
            string testPassword = "TestPassword";
            string testHash = Services.HashPassword(testPassword);
            
            if (testHash != testPassword) {
                if (Services.VerifyPassword(testPassword, testHash)) { 
                    result = true;
                }
            }
            Assert.True(result);
        }

        
    }
}