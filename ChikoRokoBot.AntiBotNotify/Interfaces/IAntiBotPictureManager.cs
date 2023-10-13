using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChikoRokoBot.AntiBotNotify.Interfaces
{
	public interface IAntiBotPictureManager
	{
		public Task<IList<string>> GetPicturesUrl();
	}
}

