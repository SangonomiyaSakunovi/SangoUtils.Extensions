namespace SangoUtils.SQLServices.MongoDBs
{
#if MongoDB
    public class MongoDBService : BaseService<MongoDBService>
    {
        private IMongoClient? _client = null;
        private IMongoDatabase? _dataBase = null;
        private string _mongoDBName = string.Empty;
        private string _mongoDBAddress = string.Empty;

        public override void OnInit()
        {
            base.OnInit();
            List<string> mongoDBConnectionInfo = ServerDBConfig.GetDBConnectionInfo();
            _mongoDBName = mongoDBConnectionInfo[0];
            _mongoDBAddress = mongoDBConnectionInfo[1];
            MongoUrlBuilder urlBuilder = new MongoUrlBuilder(_mongoDBAddress);
            _client = new MongoClient(urlBuilder.ToMongoUrl());
            _dataBase = _client.GetDatabase(_mongoDBName);
        }

        #region Add Data
        public bool AddOneData<T>(T t, string collectionName) where T : class, new()
        {
            try
            {
                bool res = false;
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    collection.InsertOne(t);
                    res = true;
                }
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddOneDataASync<T>(T t, string collectionName) where T : class, new()
        {
            try
            {
                bool res = false;
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    await collection.InsertOneAsync(t);
                    res = true;
                }
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddBatchData<T>(List<T> t, string collectionName) where T : class, new()
        {
            try
            {
                bool res = false;
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    collection.InsertMany(t);
                    res = true;
                }
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddBatchDataASync<T>(List<T> t, string collectionName) where T : class, new()
        {
            try
            {
                bool res = false;
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    await collection.InsertManyAsync(t);
                    res = true;
                }
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Data
        public UpdateResult? UpdateOneData<T>(T t, string collectionName, string objectId) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    var filter = Builders<T>.Filter.Eq("_id", objectId);
                    var list = new List<UpdateDefinition<T>>();
                    foreach (var item in t.GetType().GetProperties())
                    {
                        if (item.Name.ToLower() == "_id") continue;
                        list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(t)));
                    }
                    var updateFilter = Builders<T>.Update.Combine(list);
                    return collection.UpdateOne(filter, updateFilter);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UpdateResult?> UpdateOneDataASync<T>(T t, string collectionName, string objectId) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    var filter = Builders<T>.Filter.Eq("_id", objectId);
                    var list = new List<UpdateDefinition<T>>();
                    foreach (var item in t.GetType().GetProperties())
                    {
                        if (item.Name.ToLower() == "_id") continue;
                        list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(t)));
                    }
                    var updateFilter = Builders<T>.Update.Combine(list);
                    return await collection.UpdateOneAsync(filter, updateFilter);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpdateResult? UpdateBatchData<T>(string collectionName, Dictionary<string, string> dict, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    T t = new T();
                    var list = new List<UpdateDefinition<T>>();
                    foreach (var item in t.GetType().GetProperties())
                    {
                        if (!dict.ContainsKey(item.Name)) continue;
                        var value = dict[item.Name];
                        list.Add(Builders<T>.Update.Set(item.Name, value));
                    }
                    var updateFilter = Builders<T>.Update.Combine(list);
                    return collection.UpdateMany(filter, updateFilter);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UpdateResult?> UpdateBatchDataASync<T>(string collectionName, Dictionary<string, string> dict, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    T t = new T();
                    var list = new List<UpdateDefinition<T>>();
                    foreach (var item in t.GetType().GetProperties())
                    {
                        if (!dict.ContainsKey(item.Name)) continue;
                        var value = dict[item.Name];
                        list.Add(Builders<T>.Update.Set(item.Name, value));
                    }
                    var updateFilter = Builders<T>.Update.Combine(list);
                    return await collection.UpdateManyAsync(filter, updateFilter);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Delet Data
        public DeleteResult? DeletOneData<T>(string collectionName, string objectId) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", objectId);
                    return collection.DeleteOne(filter);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DeleteResult?> DeletOneDataASync<T>(string collectionName, string objectId) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", objectId);
                    return await collection.DeleteOneAsync(filter);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DeleteResult? DeletBatchData<T>(string collectionName, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    return collection.DeleteMany(filter);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DeleteResult?> DeletBatchDataASync<T>(string collectionName, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    return await collection.DeleteManyAsync(filter);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region LookUp Data
        public T? LookUpOneData<T>(string collectionName, string objectId, string[]? field = null) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", objectId);
                    if (field == null || field.Length == 0)
                    {
                        return collection.Find(filter).FirstOrDefault<T>();
                    }
                    var fieldList = new List<ProjectionDefinition<T>>();
                    for (int i = 0; i < field.Length; i++)
                    {
                        fieldList.Add(Builders<T>.Projection.Include(field[i]));
                    }
                    var projection = Builders<T>.Projection.Combine(fieldList);
                    fieldList?.Clear();
                    return collection.Find(filter).Project<T>(projection).FirstOrDefault<T>();
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T?> LookUpOneDataASync<T>(string collectionName, string objectId, string[]? field = null) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", objectId);
                    if (field == null || field.Length == 0)
                    {
                        return await collection.Find(filter).FirstOrDefaultAsync<T>();
                    }
                    var fieldList = new List<ProjectionDefinition<T>>();
                    for (int i = 0; i < field.Length; i++)
                    {
                        fieldList.Add(Builders<T>.Projection.Include(field[i]));
                    }
                    var projection = Builders<T>.Projection.Combine(fieldList);
                    fieldList?.Clear();
                    return await collection.Find(filter).Project<T>(projection).FirstOrDefaultAsync<T>();
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<T>? LookUpBatchData<T>(string collectionName, FilterDefinition<T> filter, string[]? field = null, SortDefinition<T>? sort = null) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    if (field == null || field.Length == 0)
                    {
                        if (sort == null)
                        {
                            return collection.Find(filter).ToList();
                        }
                        else
                        {
                            return collection.Find(filter).Sort(sort).ToList();
                        }
                    }
                    var fieldList = new List<ProjectionDefinition<T>>();
                    for (int i = 0; i < field.Length; i++)
                    {
                        fieldList.Add(Builders<T>.Projection.Include(field[i]));
                    }
                    var projection = Builders<T>.Projection.Combine(fieldList);
                    fieldList.Clear();
                    if (sort == null)
                    {
                        return collection.Find(filter).Project<T>(projection).ToList();
                    }
                    else
                    {
                        return collection.Find(filter).Project<T>(projection).Sort(sort).ToList();
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<T>?> LookUpBatchDataASync<T>(string collectionName, FilterDefinition<T> filter, string[]? field = null, SortDefinition<T>? sort = null) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    if (field == null || field.Length == 0)
                    {
                        if (sort == null)
                        {
                            return await collection.Find(filter).ToListAsync();
                        }
                        else
                        {
                            return await collection.Find(filter).Sort(sort).ToListAsync();
                        }
                    }
                    var fieldList = new List<ProjectionDefinition<T>>();
                    for (int i = 0; i < field.Length; i++)
                    {
                        fieldList.Add(Builders<T>.Projection.Include(field[i]));
                    }
                    var projection = Builders<T>.Projection.Combine(fieldList);
                    fieldList?.Clear();
                    if (sort == null)
                    {
                        return await collection.Find(filter).Project<T>(projection).ToListAsync();
                    }
                    else
                    {
                        return await collection.Find(filter).Project<T>(projection).Sort(sort).ToListAsync();
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Count
        public long GetCount<T>(string collectionName, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    return collection.CountDocuments(filter);
                }
                return long.MinValue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<long> GetCountAsync<T>(string collectionName, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = _dataBase?.GetCollection<T>(collectionName);
                if (collection != null)
                {
                    return await collection.CountDocumentsAsync(filter);
                }
                return long.MinValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
#endif
}
