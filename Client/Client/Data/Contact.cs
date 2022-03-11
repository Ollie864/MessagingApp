using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Client.Data
{
    public class Contact
    {
        //Creates the field of the database of users with Id being the public key
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }

    }
}
