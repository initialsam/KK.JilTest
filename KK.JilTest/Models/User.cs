using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KK.JilTest.Models
{
    [Serializable]
    public class User
    {
        public Guid Id { get; set; }

        public DateTime RegDate { get; set; }

        public string Name { get; set; }

        public decimal Score { get; set; }

        public string Display
        {
            get
            {
                return string.Format(
                    "{0} / {1:yyyy-MM-dd} / {2:N0}",
                    Name, RegDate, Score);
            }
        }

        public List<User> GenSimData()
        {
            List<User> lst = new List<User>();
            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                lst.Add(new User()
                {
                    Id = Guid.NewGuid(),
                    RegDate = DateTime.Today.AddDays(-rnd.Next(5000)),
                    Name = "User" + i,
                    Score = rnd.Next(65535)
                });
            }
            return lst;
        }
    }
}