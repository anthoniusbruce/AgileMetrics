using System.Text;
using System.Text.Json;
using AdoAnalysisServerApis;
using AgileMetricsRules;
using AgileMetricsServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgileMetricsServer.Api
{
    [ApiController]
    public class CycleTimeController : ControllerBase
    {
        IAdoAnalysisService AdoService { get; set; }

        public CycleTimeController(IAdoAnalysisService service)
        {
            AdoService = service;
        }

        [HttpPost]
        [Route("api/charts/cycletime/v1")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult?> CycleTimeChartData()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var readAmt = await reader.ReadToEndAsync();

                CycleTimeApiJson? json;
                try
                {
                    json = JsonSerializer.Deserialize<CycleTimeApiJson>(readAmt);
                }
                catch (JsonException)
                {
                    json = null;
                }

                if (json == null || string.IsNullOrWhiteSpace(json.token) || string.IsNullOrWhiteSpace(json.workItemType) ||
                    json.startingDate == null || json.endingDate == null || string.IsNullOrWhiteSpace(json.team))
                {
                    return BadRequest();
                }

                var workItemType = AdoAnalysisService.GetWorkItemTypeQuerySubstring(json.workItemType);

                if (string.IsNullOrWhiteSpace(workItemType))
                    return BadRequest();

                ScatterPlotJsonRecord? scatterPlots;
                try
                {
                    scatterPlots = await AdoService.GetCycleTimeScatterPlotData(workItemType, json.startingDate.Value, json.endingDate.Value, json.tags ?? string.Empty, json.team, json.token);
                }
                catch (HttpRequestException e)
                {
                    if (e.StatusCode == null)
                        return StatusCode(500);

                    return StatusCode((int)e.StatusCode);
                }
                catch (InvalidOperationException)
                {
                    // log this someday
                    return StatusCode(500);
                }

                if (scatterPlots == null)
                    return BadRequest();

                if (scatterPlots.NotAuthorized)
                    return Unauthorized();

                var points = ScatterPlot.CalculateCycleTimeScatterPlotPoints(scatterPlots);
                var percentiles = ScatterPlot.CalculateCycleTimePercentiles(points);

                var workItemTypeString = workItemType.Replace("+", " ").Replace("%27", "'");
                var title = string.Format("ADO team: {0} {1} From: {2} To: {3}", json.team, workItemTypeString, json.startingDate.Value.ToString("d"), json.endingDate.Value.ToString("d"));

                if (!string.IsNullOrWhiteSpace(json.tags))
                    title = string.Format("{0} exclude: {1}", title, json.tags);

                var anon = new
                {
                    title = title,
                    points = points,
                    percentiles = percentiles
                };

                return Ok(anon);
            }
        }
    }
}
