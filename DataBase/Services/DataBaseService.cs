using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataBase.Managers
{
	public class DataBaseService
	{
		public DataBaseService() { }

		public void WriteDB(string patient)
		{
			string currentDirectory = Directory.GetCurrentDirectory();
			string databasesPath = Path.Combine(currentDirectory, "..", "DataBase", "DataBases");
			Directory.CreateDirectory(databasesPath);

			string filePath = Path.Combine(databasesPath, "patients.json");

			List<JsonElement> patientsList = new();

			if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
			{
				try
				{
					string existingJson = File.ReadAllText(filePath);
					patientsList = JsonSerializer.Deserialize<List<JsonElement>>(existingJson)
						?? new List<JsonElement>();
					Log.Information("JSON file has been read succesfully from {FilePath}", filePath);
				}
				catch (Exception ex)
				{
					Log.Error(ex, "Failed to read the JSON file");
					patientsList = new List<JsonElement>();
				}
			}

			// Convertimos el nuevo paciente (string JSON) a JsonElement
			JsonElement newPatient = JsonSerializer.Deserialize<JsonElement>(patient);
			patientsList.Add(newPatient);

			// Guardamos la lista actualizada
			string updatedJson = JsonSerializer.Serialize(patientsList, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(filePath, updatedJson);
			Log.Information("New Patient added to the jason file");
		}


		public string ReadDB()
		{
			string currentDirectory = Directory.GetCurrentDirectory();
			string databasesPath = Path.Combine(currentDirectory, "..", "DataBase", "DataBases");
			string filePath = Path.Combine(databasesPath, "patients.json");

			if (!File.Exists(filePath))
			{
				Log.Warning("the file {FilePath} doesn't exist", filePath);
				return "El archivo patients.json no existe.";
			}

			return File.ReadAllText(filePath);
		}

		public void UpdateDB(string ci, string nuevoPacienteJson)
		{
			string currentDirectory = Directory.GetCurrentDirectory();
			string databasesPath = Path.Combine(currentDirectory, "..", "DataBase", "DataBases");
			string filePath = Path.Combine(databasesPath, "patients.json");

			var json = File.ReadAllText(filePath);
			var pacientes = JsonSerializer.Deserialize<List<JsonElement>>(json) ?? new List<JsonElement>();

			// Convertimos el nuevo paciente en JsonElement
			JsonElement nuevoPaciente = JsonSerializer.Deserialize<JsonElement>(nuevoPacienteJson);

			// Reemplazamos el paciente con el mismo CI
			var actualizados = pacientes
				.Where(p => !(p.TryGetProperty("CI", out var ciProp) && ciProp.GetString() == ci))
				.ToList();

			actualizados.Add(nuevoPaciente);

			string jsonActualizado = JsonSerializer.Serialize(actualizados, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(filePath, jsonActualizado);
			Log.Information("Patien has been updated to the file succesfully");
		}


		public void DeleteDB(string ci)
		{
			string currentDirectory = Directory.GetCurrentDirectory();
			string databasesPath = Path.Combine(currentDirectory, "..", "DataBase", "DataBases");
			string filePath = Path.Combine(databasesPath, "patients.json");

			var json = File.ReadAllText(filePath);
			var pacientes = JsonSerializer.Deserialize<List<JsonElement>>(json) ?? new List<JsonElement>();

			// Filtrar la lista quitando el paciente con el CI indicado
			var actualizados = pacientes
				.Where(p => p.TryGetProperty("CI", out var ciProp) && ciProp.GetString() != ci)
				.ToList();

			string jsonActualizado = JsonSerializer.Serialize(actualizados, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(filePath, jsonActualizado);
			Log.Information("Patient has been deleted from the file successfully");
		}

	}
}
