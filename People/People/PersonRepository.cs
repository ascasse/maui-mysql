using People.Models;
using SQLite;

namespace People
{
    public class PersonRepository
    {
        string _dbPath;
        private SQLiteAsyncConnection conn;

        public string StatusMessage { get; set; }

        private async Task Init()
        {
            if (conn != null)
                return;
            conn = new SQLiteAsyncConnection(_dbPath);
            await conn.CreateTableAsync<Person>();
        }

        public PersonRepository(string dbPath)
        {
            _dbPath = dbPath;                        
        }

        public async Task AddNewPerson(string name)
        {            
            int result = 0;
            try
            {
                await Init();

                // basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                // TODO: Insert the new person into the database
                result = await conn.InsertAsync(new Person { Name = name });

                StatusMessage = $"{result} record(s) added (Name: {name})";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to add {name}. Error: {ex.Message}";
            }

        }

        public async Task<List<Person>> GetAllPeople()
        {
            try
            {
                await Init();
                return await conn.Table<Person>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<Person>();
        }
    }
}
