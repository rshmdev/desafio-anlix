using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly PatientService _patientService;

        public PatientsController(PatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients(
            string cpf = null,
            string namePart = null,
            DateTime? date = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            double? minValue = null,
            double? maxValue = null)
        {
            var patients = await _patientService.GetPatientsAsync();

            // Filtrar por CPF
            if (!string.IsNullOrEmpty(cpf))
            {
                patients = patients.Where(p => p.Cpf == cpf).ToList();
            }

            // Filtrar por nome
            if (!string.IsNullOrEmpty(namePart))
            {
                patients = patients.Where(p => p.Nome.Contains(namePart, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filtrar por data específica
            if (date.HasValue)
            {
                var cardiaco = await _patientService.GetIndicesByDateAsync("Helpers/indice_cardiaco", date.Value);
                var pulmonar = await _patientService.GetIndicesByDateAsync("Helpers/indice_pulmonar", date.Value);

                return Ok(new
                {
                    Date = date,
                    Cardiaco = cardiaco,
                    Pulmonar = pulmonar
                });
            }

            // Filtrar por intervalo de datas
            if (startDate.HasValue && endDate.HasValue)
            {
                var cardiaco = await _patientService.GetIndicesByDateRangeAsync("Helpers/indice_cardiaco", cpf, startDate.Value, endDate.Value);
                var pulmonar = await _patientService.GetIndicesByDateRangeAsync("Helpers/indice_pulmonar", cpf, startDate.Value, endDate.Value);

                return Ok(new
                {
                    Cpf = cpf,
                    StartDate = startDate,
                    EndDate = endDate,
                    Cardiaco = cardiaco,
                    Pulmonar = pulmonar
                });
            }

            if (minValue.HasValue && maxValue.HasValue)
            {
                var cardiaco = await _patientService.GetLatestCardiacoIndexByValueRangeAsync("Helpers/indice_cardiaco", cpf, minValue.Value, maxValue.Value);
                var pulmonar = await _patientService.GetLatestPulmonarIndexByValueRangeAsync("Helpers/indice_pulmonar", cpf, minValue.Value, maxValue.Value);

                return Ok(new
                {
                    Cpf = cpf,
                    MinValue = minValue,
                    MaxValue = maxValue,
                    Cardiaco = cardiaco?.IndCard,
                    Pulmonar = pulmonar?.IndPulm
                });
            }

            return Ok(patients);
        }
    }
}
