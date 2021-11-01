using CoronavirusWebScaper.Controllers.Mapping;
using MongoDB.Driver;

namespace CoronavirusWebScaper.Services
{
	public interface IMongoService
	{
		IMongoCollection<CoronaData> GetAllRecords();

		CoronaData GetFirstRecordFromMongoCollection();
	}
}
