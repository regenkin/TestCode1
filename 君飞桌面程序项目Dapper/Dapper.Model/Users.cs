using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dapper.Model
{
    public class Users
    {
        public int? Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
