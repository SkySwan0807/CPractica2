using BusinessLogic.Managers;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Models;
using System.Text.Json;

namespace Practica2.Controllers
{
	[ApiController]
	[Route("patient")]
	public class PatientsController : ControllerBase
	{
		private readonly PatientsManager _manager;

		public PatientsController(PatientsManager manager)
		{
			_manager = manager;
		}

		[HttpGet]
		[Route("")]
		public IEnumerable<Patients> GetAll()
		{
			return _manager.GetAllPatients();
		}

		[HttpGet]
		[Route("{PatientId}")]
		public dynamic GetById(string PatientId)
		{
			var patient = _manager.GetById(PatientId);
			return patient;
		}

		[HttpPost]
		[Route("add")]
		public Patients CreatePatient([FromBody] Patients newPatient)
		{

			var patient = _manager.Add(newPatient.Name, newPatient.LastName);
			//Console.WriteLine($"{manager._patients[3].CI}");
			
			return patient;
		}

		[HttpPut]
		[Route("{PatientId}")]
		public dynamic UpdateById(string PatientId, [FromBody] Patients p)
		{
			var patient = _manager.UpdateById(PatientId, p.Name, p.LastName);
			return patient;
		}

		[HttpDelete]
		[Route("{PatientId}")]
		public dynamic DeleteById(string PatientId)
		{
			return _manager.Delete(PatientId);
		}

		[HttpPost]
		[Route("collect-gift")]
		public dynamic CollectGift([FromBody]Patients PatientId)
		{
			var gift = _manager.CollectGiftForPatient(PatientId);
			
			return gift;
		}
	}
	
}
