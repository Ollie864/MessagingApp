using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;

namespace Client.Data
{
    public class Database
    {
        //This is the connection to the database
        private readonly SQLiteAsyncConnection _database;
        public Database(string dbPath)
        {
            //Giving the database the file path
            _database = new SQLiteAsyncConnection(dbPath);
            //Creates the database if the table does not already exist
            _database.CreateTableAsync<Contact>();
        }
        public Task<List<Contact>> GetContacts()
        {
            return _database.Table<Contact>().ToListAsync();
        }
        //This method will allow for creating new accounts
        public Task<int> SaveContact(Contact contact)
        {
            return _database.InsertAsync(contact);
        }
        //This will allow for updating uses accounts
        public Task<int> UpdateContact(Contact contact)
        {
            return _database.UpdateAsync(contact);
        }
        //This will allow for deleting account
        public Task<int> DeleteContact(int id)
        {
            return _database.DeleteAsync<Contact>(id);
        }
        public Task<List<Contact>> GetSpecificContact(string username)
        {
            return _database.Table<Contact>().Where(i => i.Name == username).ToListAsync();
        }
        public Task<List<Contact>> GetSpecificContact(int id)
        {
            return _database.Table<Contact>().Where(i => i.Id == id).ToListAsync();
        }

        public Task<int> DeleteAllContact()
        {
            return _database.DeleteAllAsync<Contact>();
        }
    }
}
