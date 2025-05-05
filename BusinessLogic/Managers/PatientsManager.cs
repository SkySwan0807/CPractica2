using BusinessLogic.Models;
using Serilog;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DataBase.Managers;
using System.Text.Json;

namespace BusinessLogic.Managers
{
	public class PatientsManager
	{
		DataBaseService db = new DataBaseService();
		List<Patients> _patients;
		private static int _nextId;
		private static readonly Random _random = new();

		public PatientsManager()
		{
			string json = db.ReadDB();

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};

			try
			{
				// Si comienza con [, asumimos que es una lista
				if (json.TrimStart().StartsWith("["))
				{
					_patients = JsonSerializer.Deserialize<List<Patients>>(json, options) ?? new List<Patients>();
				}
				else
				{
					// Si es un solo objeto, lo envolvemos en una lista
					var singlePatient = JsonSerializer.Deserialize<Patients>(json, options);
					_patients = singlePatient != null ? new List<Patients> { singlePatient } : new List<Patients>();
				}
			}
			catch
			{
				_patients = new List<Patients>(); // Si el JSON está malformado
			}
			_nextId = ObtenerCIMayor() + 1;
		}

		public string GetRandomBloodGroup()
		{
			var bloodGroups = Enum.GetValues(typeof(BloodGroup));
			return bloodGroups.GetValue(_random.Next(bloodGroups.Length)).ToString();

		}

		public int ObtenerCIMayor()
		{
			if (_patients == null || _patients.Count == 0)
				return 1000;

			return _patients
				.Select(p => int.TryParse(p.CI, out var ciNum) ? ciNum : 0)
				.Max();
		}

		public dynamic GetAllPatients()
		{
			Log.Information("Getting all patients");
			if (_patients.Count == 0)
			{
				Log.Error("There're no patients registered yet");
				return (object)"No patients registered";
			}
			else
			{
				Log.Information("Patients found successfully");
				return _patients;
			}
		}

		public dynamic GetById(string PatientId)
		{
			var patient = _patients.FirstOrDefault(p => p.CI == PatientId);
			if (patient == null)
			{
				Log.Error("Patient couldn't be found");
				return (object)"Patient not found";
			} else 
			{
				Log.Information("Patient found successfully");
				return patient;
			}
			
		}

		public dynamic Add(string n, string a)
		{

			if (string.IsNullOrWhiteSpace(n) || string.IsNullOrWhiteSpace(a))
			{
				Log.Error("Name and LastName are required");
				return "Name and LastName are required";
			}
			var patient = new Patients()
			{
				CI = $"{_nextId++}",
				Name = n,
				LastName = a,
				BloodGroup = GetRandomBloodGroup()
			};

			var options = new JsonSerializerOptions { WriteIndented = true };
			string json = JsonSerializer.Serialize(patient, options);
			db.WriteDB(json);

			Log.Information("Patient added successfully");
			return patient;
		}

		public dynamic UpdateById(string ci, string n, string a)
		{
			var patient = _patients.FirstOrDefault(p => p.CI == ci);
			if (patient == null)
			{
				Log.Error("Patient couldn't be updated");
				return (object)"Patient not found";
			}
			else
			{
				patient.Name = n;
				patient.LastName = a;
				var options = new JsonSerializerOptions { WriteIndented = true };
				string json = JsonSerializer.Serialize(patient, options);
				db.UpdateDB(ci, json);

				Log.Information("Patient modified successfully");
				return patient;
			}
		}

		public dynamic Delete(string PatientId)
		{
			var patient = _patients.FirstOrDefault(p => p.CI == PatientId);
			if (patient == null)
			{
				Log.Error("Patient couldn't be deleted");
				return (object)"Patient not found";
			}
			else
			{
				db.DeleteDB(PatientId);

				Log.Information("Patient deleted successfully");
				return (object)$"Patient with id = {PatientId} has been deleted";
			}
		}

		public dynamic CollectGiftForPatient(Patients patients)
		{
			var collectedGift = new Electronic();
			GiftManager gm = new GiftManager();
			List<Electronic> _giftList = gm.GetGiftList();
			int id = int.Parse(patients.CI);
			id = id - 1000;
			if (id > 13)
			{
				Log.Error("Patient didn't give a gift");
				return (object)"No gift given";
			} else
			{
				collectedGift = _giftList[(id-1)];
				Log.Information("Gift recived successfully");
				return collectedGift;
			};
		}

	}
	public enum BloodGroup
	{
		A_Positive,
		A_Negative,
		B_Positive,
		B_Negative,
		AB_Positive,
		AB_Negative,
		O_Positive,
		O_Negative
	}
}
