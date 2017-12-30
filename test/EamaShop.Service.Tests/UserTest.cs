using System;
using System.IO;
using Xunit;

namespace EamaShop.Service.Test
{
    public class UserTest
    {

        [Fact]
        public void Regiter_User_Test()
        {
            var current = Directory.GetCurrentDirectory();
            var path = Path.Combine(current, "../wwwroot/images");
            Directory.CreateDirectory(path);
        }
    }
}
