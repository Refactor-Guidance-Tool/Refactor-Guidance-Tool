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
	private readonly string _databaseOutputDirectory;

	private IDocumentCollection<DatabaseData> Databases => this._dataStore.GetCollection<DatabaseData>();

	public IReadOnlyList<DatabaseData> DatabaseList => this.Databases.AsQueryable().ToList();

	public DatabaseDataStore(string outputDirectory) {
		this._databaseOutputDirectory = outputDirectory;
		this._dataStore = new DataStore($"{this._databaseOutputDirectory}/databases.json");
	}
	
	private void Insert(DatabaseData databaseData) {
		this._dataStore.InsertItem(databaseData.Uuid.ToString(), databaseData);
	}

	public DatabaseData Insert(string databasePath) {
		var databaseData = new DatabaseData(databasePath);
		this.Insert(databaseData);
		return databaseData;
	}

	public void Update(DatabaseData databaseData) {
		this._dataStore.UpdateItem(databaseData.Uuid.ToString(), databaseData);
	}

	public DatabaseData? FindDatabaseByPath(string databasePath) {
		return this.Databases.Find(data => data.Path == databasePath).FirstOrDefault();
	}

	private void DeleteByUuid(Guid uuid) {
		this._dataStore.DeleteItem(uuid.ToString());
	}

	public void Delete(DatabaseData databaseData) {
		if (!Directory.Exists(databaseData.Path))
			return;

		Directory.Delete(databaseData.Path, true);
		this.DeleteByUuid(databaseData.Uuid);
	}

	public int RemoveAll() {
		if (!Directory.Exists(this._databaseOutputDirectory))
			return 0;

		var databaseCount = Count();

		Directory.Delete(this._databaseOutputDirectory, true);
		this.Databases.DeleteMany(data => true);

		return databaseCount;
	}

	private int Count() {
		return this.Databases.Count;
	}
}