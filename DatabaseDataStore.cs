using System.Collections.ObjectModel;
using JsonFlatFileDataStore;

namespace RefactorGuidanceTool; 

public class DatabaseDataStore {
	public class DatabaseData {
		public Guid Uuid { get; set; }
		public string Path { get; set; }

		public DatabaseData(Guid uuid, string path) {
			this.Uuid = uuid;
			this.Path = path;
		}
	}

	private readonly DataStore _dataStore;
	private readonly string _databaseOutputDirectory;

	private IDocumentCollection<DatabaseData> Databases => this._dataStore.GetCollection<DatabaseData>();

	public IReadOnlyList<DatabaseData> DatabaseList => this.Databases.AsQueryable().ToList();

	public DatabaseDataStore(string outputDirectory) {
		this._databaseOutputDirectory = outputDirectory;
		
		Utils.EnsureDirectoryExists(this._databaseOutputDirectory);
		this._dataStore = new DataStore($"{this._databaseOutputDirectory}/databases.json");
	}
	
	private void Insert(DatabaseData databaseData) {
		this.Databases.InsertOne(databaseData);
	}

	public DatabaseData Insert(string databasePath) {
		var databaseData = new DatabaseData(Guid.NewGuid(), databasePath);
		this.Insert(databaseData);
		return databaseData;
	}

	public void Update(DatabaseData databaseData) {
		this.Databases.UpdateOne(databaseData.Uuid, databaseData);
	}

	public DatabaseData? FindDatabaseByPath(string databasePath) {
		return this.Databases.AsQueryable().FirstOrDefault(data => data.Path == databasePath);
	}

	private void DeleteFromStore(Guid uuid) {
		this.Databases.DeleteMany(data => data.Uuid == uuid);
	}

	private void DeleteFromStore(string databasePath) {
		this.Databases.DeleteMany(data => data.Path == databasePath);
	}

	public void Delete(DatabaseData databaseData) {
		Utils.SafeDeleteDirectory(databaseData.Path, true);
		this.DeleteFromStore(databaseData.Uuid);
	}

	public void Delete(string databasePath) {
		Utils.SafeDeleteDirectory(databasePath, true);
		this.DeleteFromStore(databasePath);
	}

	public int RemoveAll() {
		if (!Directory.Exists(this._databaseOutputDirectory))
			return 0;

		var databaseCount = this.Count();

		Directory.Delete(this._databaseOutputDirectory, true);
		Directory.CreateDirectory(this._databaseOutputDirectory);

		this.Databases.DeleteMany(data => true);

		return databaseCount;
	}

	private int Count() {
		return this.Databases.Count;
	}
}