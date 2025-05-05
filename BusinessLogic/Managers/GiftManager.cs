using Services.ExternalServices;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Managers
{
	public class GiftManager
	{

		public GiftManager() { }

		public List<Electronic> GetGiftList()
		{
			GiftSerrvice gs = new GiftSerrvice();
			return gs.GetAllGifts().Result;
		}
	}
}
