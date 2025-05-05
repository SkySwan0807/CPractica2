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

namespace BusinessLogic.Managers
{
	public class PatientsManager
	{
		public List<Patients>? _patients;
		private static int _nextId = 1011;
		private static readonly Random _random = new();

		public PatientsManager()
		{
			if (_patients == null)
			{
				_patients = new List<Patients>
				{
					new Patients()
					{
						CI = $"{_nextId++}",
						Name = "Juan",
						LastName = "Perez",
						BloodGroup = GetRandomBloodGroup()
					},
					new Patients()
					{
						CI = $"{_nextId++}",
						Name = "Mateo",
						LastName = "Alvarez",
						BloodGroup = GetRandomBloodGroup()
					},
					new Patients()
					{
						CI = $"{_nextId++}",
						Name = "Amir",
						LastName = "Solano",
						BloodGroup = GetRandomBloodGroup()
					}
				};
			}
		}

		public string GetRandomBloodGroup()
		{
			var bloodGroups = Enum.GetValues(typeof(BloodGroup));
			return bloodGroups.GetValue(_random.Next(bloodGroups.Length)).ToString();

		}

		public List<Patients> GetAllPatients()
		{
			Log.Information("Getting all patients");
			return _patients;
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
			_patients.Add(patient);
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
				_patients.Remove(patient);
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
				Log.Error("Patient didn't have a gift");
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
