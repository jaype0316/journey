using MongoDB.Bson;
using MongoDB.Driver;

namespace Journey.Repository.MongoDb
{
    public sealed class JourneyMongoClient : MongoClient
    {
        static string _cnn = "mongodb+srv://journeyadmin:JSn9i69D3OBy6tQt@cluster0.hjar3vm.mongodb.net/?retryWrites=true&w=majority";

        public JourneyMongoClient() : base(_cnn) { }


        public IEnumerable<BsonDocument> Connect()
        {
            var dbs = this.ListDatabases().ToList();
            return dbs;
            
        }
    }
}