using BusinessLogic.Models;
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
		private static int _nextId = 1000;
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
			return _patients;
		}

		public dynamic GetById(string PatientId)
		{
			var patient = _patients.FirstOrDefault(p => p.CI == PatientId);
			return patient ?? (object)"Patient not found";
		}

		public dynamic Add(string n, string a)
		{
			if (string.IsNullOrWhiteSpace(n) || string.IsNullOrWhiteSpace(a))
			{
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
			return patient;
		}

		public dynamic UpdateById(string ci, string n, string a)
		{
			var patient = _patients.FirstOrDefault(p => p.CI == ci);
			if (patient == null)
			{
				return (object)"Patient not found";
			}
			else
			{
				patient.Name = n;
				patient.LastName = a;
				return patient;
			}
		}

		public dynamic Delete(string PatientId)
		{
			var patient = _patients.FirstOrDefault(p => p.CI == PatientId);
			if (patient == null)
			{
				return (object)"Patient not found";
			}
			else
			{
				_patients.Remove(patient);
				return (object)$"Patient with id = {PatientId} has been deleted";
			}
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
