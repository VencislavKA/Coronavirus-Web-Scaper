using Coronavirus_Web_Scaper.Controllers.Mapping;
using MongoDB.Driver;

namespace Coronavirus_Web_Scaper.Services
{
	public interface IMongoService
	{
		IMongoCollection<RootObject> GetCollection();

		RootObject GetRootobject();
	}
}
