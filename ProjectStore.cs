using JsonFlatFileDataStore;
using System.Linq;
using OneOf;
using RefactorGuidanceTool.Models;

namespace RefactorGuidanceTool;

public class ProjectStore {
	public class ProjectData {
		public Guid Uuid { get; set; }
		public string ProjectPath { get; set; }
		public ProjectLanguage Language { get; set; }

		public ProjectData() { }

		public ProjectData(Project project) {
			this.Uuid = project.Uuid;
			this.ProjectPath = project.ProjectPath;
			this.Language = project.ProjectLanguage;
		}
	}

	private readonly ProjectFactory _projectFactory;
	private readonly DataStore _dataStore;

	private IDocumentCollection<ProjectData> Projects => this._dataStore.GetCollection<ProjectData>();

	public ProjectStore(ProjectFactory projectFactory, string outputDirectory) {
		this._projectFactory = projectFactory;

		Utils.EnsureDirectoryExists(outputDirectory);
		this._dataStore = new DataStore($"{outputDirectory}/databases.json");
	}

	public bool Insert(Project project) {
		// var projectWithSamePath = this.Projects
		// 	.AsQueryable()
		// 	.ToList()
		// 	.FirstOrDefault(projectData => projectData.ProjectPath == project.ProjectPath);
		// if (projectWithSamePath != null)
		// 	return false;

		var projectData = new ProjectData(project);
		this.Projects.InsertOne(projectData);

		return true;
	}

	public void Delete(Project project) {
		this.Projects.DeleteMany(data => data.Uuid == project.Uuid);
	}

	public record ProjectNotFound() { }

	public OneOf<Project, ProjectNotFound> GetProjectByUuid(Guid uuid) {
		return this.GetProjectByUuid(uuid.ToString());
	}

	public OneOf<Project, ProjectNotFound> GetProjectByUuid(string projectUuid) {
		var projectData = this.Projects.Find(projectUuid).FirstOrDefault();

		if (projectData == null)
			return new ProjectNotFound();

		var project = this._projectFactory.CreateProject(projectData.Uuid, projectData.Language, projectData.ProjectPath);

		return project;
	}

	public IReadOnlyList<Project> GetProjects() {
		return this.Projects
			.AsQueryable()
			.ToList()
			.Select(projectData => this.GetProjectByUuid(projectData.Uuid))
			.Select(status => status.Match(
				project => project,
				notFound => null!))
			.Where(project => project != null)
			.ToList();
	}

	private int Count() {
		return this.Projects.Count;
	}
}