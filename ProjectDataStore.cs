using System.Collections.ObjectModel;
using JsonFlatFileDataStore;

namespace RefactorGuidanceTool;

public class ProjectDataStore {
	public class ProjectData {
		public Guid Uuid { get; }
		public string Path { get; set; }
		public string DatabasePath { get; set; }

		public ProjectData(string path, string databasePath) {
			this.Uuid = new Guid();
			this.Path = path;
			this.DatabasePath = this.Uuid.ToString();
		}
	}

	private readonly DataStore _dataStore;

	public IDocumentCollection<ProjectData> Projects => this._dataStore.GetCollection<ProjectData>();

	public ProjectDataStore(string outputDirectory) {
		this._dataStore = new DataStore($"{outputDirectory}/databases.json");
	}

	public IReadOnlyList<ProjectData> GetProjects() {
		var collection = this._dataStore.GetCollection<ProjectData>();
		var databases = collection.AsQueryable().AsEnumerable().ToList();

		return databases;
	}

	public void Insert(ProjectData projectData) {
		this._dataStore.InsertItem(projectData.Uuid.ToString(), projectData);
	}

	public void Update(ProjectData projectData) {
		this._dataStore.UpdateItem(projectData.Uuid.ToString(), projectData);
	}
}