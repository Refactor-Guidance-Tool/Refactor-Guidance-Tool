using System.Collections.ObjectModel;
using JsonFlatFileDataStore;

namespace RefactorGuidanceTool; 

public class DatabaseDataStore {
	public class DatabaseData {
		public Guid Uuid { get; }
		public string Path { get; set; }

		public DatabaseData(string path) {
			this.Uuid = new Guid();
			this.Path = path;
		}
	}

	private readonly DataStore _dataStore;

	public IDocumentCollection<DatabaseData> Databases => this._dataStore.GetCollection<DatabaseData>();

	public DatabaseDataStore(string outputDirectory) {
		this._dataStore = new DataStore($"{outputDirectory}/databases.json");
	}

	public IReadOnlyList<DatabaseData> GetDatabases() {
		var collection = this._dataStore.GetCollection<DatabaseData>();
		var databases = collection.AsQueryable().AsEnumerable().ToList();

		return databases;
	}

	public void Insert(DatabaseData databaseData) {
		this._dataStore.InsertItem(databaseData.Uuid.ToString(), databaseData);
	}

	public void Update(DatabaseData databaseData) {
		this._dataStore.UpdateItem(databaseData.Uuid.ToString(), databaseData);
	}
}