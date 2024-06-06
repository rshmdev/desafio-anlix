using System.Globalization;
using CsvHelper;
using Newtonsoft.Json;
using backend.Models;


namespace backend.Services
{
    public class PatientService
    {
        public async Task<List<PatientModel>> GetPatientsAsync()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "patients.json");
            var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<List<PatientModel>>(jsonData);
        }

        public async Task<List<CardiacoModel>> GetIndicesByDateAsync(string path, DateTime date)
        {
            var indices = await GetIndicesAsync<CardiacoModel>(path);
            return indices.Where(x => x.Epoch.Date == date).ToList();
        }

        public async Task<List<CardiacoModel>> GetIndicesByDateRangeAsync(string path, string cpf, DateTime startDate, DateTime endDate)
        {
            var indices = await GetIndicesAsync<CardiacoModel>(path);
            return indices.Where(x => x.Cpf == cpf && x.Epoch.Date >= startDate && x.Epoch.Date <= endDate).ToList();
        }

        public async Task<CardiacoModel> GetLatestCardiacoIndexByValueRangeAsync(string path, string cpf, double minValue, double maxValue)
        {
            var indices = await GetIndicesAsync<CardiacoModel>(path);
            return indices.Where(x => x.Cpf == cpf && x.IndCard >= minValue && x.IndCard <= maxValue).OrderByDescending(x => x.Epoch).FirstOrDefault();
        }

        public async Task<PulmonarModel> GetLatestPulmonarIndexByValueRangeAsync(string path, string cpf, double minValue, double maxValue)
        {
            var indices = await GetIndicesAsync<PulmonarModel>(path);
            return indices.Where(x => x.Cpf == cpf && x.IndPulm >= minValue && x.IndPulm <= maxValue).OrderByDescending(x => x.Epoch).FirstOrDefault();
        }

        public async Task<List<T>> GetIndicesAsync<T>(string path)
        {
            var files = Directory.GetFiles(path, "*.csv");
            var allIndices = new List<T>();

            foreach (var file in files)
            {
                var indices = await ReadCsvAsync<T>(file);
                allIndices.AddRange(indices);
            }

            return allIndices;
        }

        private async Task<List<T>> ReadCsvAsync<T>(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<T>().ToList();
            }
        }
    }
}
