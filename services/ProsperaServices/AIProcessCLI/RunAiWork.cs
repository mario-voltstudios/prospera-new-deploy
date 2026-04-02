using System.Globalization;
using System.Text;
using System.Text.Json;
using AIProcess;
using AIProcessCLI.Models;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AIProcessCLI;

public class RunAiWork(ProcessAI processAi, ILogger<RunAiWork> logger, IHttpClientFactory httpClientFactory) : AsyncCommand
{
    private readonly string[] _codesToCalculate = ["260", "261", "262", "263", "249"];
    private readonly Lock expression = new();
    
    public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        await processAi.SearchLocation("hostipal general 200", cancellationToken);
        // // var result2 = await processAi.ExecuteIdBackExtractionAsync("/Users/rafaelmoreira/Desktop/d0edd4a5-e485-4660-bee9-bb5dc0f9ff40.jpeg", cancellationToken);
        // // var result2 = await processAi.ExecuteIdExtractionAsync("/Users/rafaelmoreira/Desktop/ff5ca40b-0c17-4feb-a7aa-21ea0fa681de.jpeg", cancellationToken);
        // var result2 = await processAi.ExecuteTabletExtractionAsync("/Users/rafaelmoreira/Desktop/prospera/files/690099775_ROSM550609RLA.png", cancellationToken);
        //
        
        
        return 0;
        
        var httpClient = httpClientFactory.CreateClient("s3-download");
        
        logger.LogInformation("initiating AI document processing...");
        // var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "prospera","image.jpg");
        var directoryCsv = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "prospera","POLIZAS-5250.csv");
        var saveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "prospera","result.html");
        var saveDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "prospera");

        List<UnableToProcess> unableToProcess = [];
        List<Processed> processed = [];
        List<Completed> completed = [];

        if (File.Exists(Path.Combine(saveDirectoryPath, "completed.json")))
        {
            var file = await File.ReadAllTextAsync(Path.Combine(saveDirectoryPath, "completed.json"), cancellationToken);

            completed.AddRange(JsonSerializer.Deserialize<List<Completed>>(file)!);
            
        }
        
        if (File.Exists(Path.Combine(saveDirectoryPath, "processed.json")))
        {
            var file = await File.ReadAllTextAsync(Path.Combine(saveDirectoryPath, "processed.json"), cancellationToken);
            processed.AddRange(JsonSerializer.Deserialize<List<Processed>>(file)!);
        }
        
        if (File.Exists(Path.Combine(saveDirectoryPath, "processed-failed.json")))
        {
            var file = await File.ReadAllTextAsync(Path.Combine(saveDirectoryPath, "processed-failed.json"), cancellationToken);

            unableToProcess.AddRange(JsonSerializer.Deserialize< List<UnableToProcess>>(file)!);
            unableToProcess.RemoveAll(x => completed.Any(c => c.PolicyNumber == x.PolicyNumber));
        }
        
        
        await AnsiConsole
            .Status()
            .Spinner(Spinner.Known.Dots4)
            .StartAsync("Loading csv records", async ctx =>
            {
                using var reader = new StreamReader(directoryCsv);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
                var records = csv.GetRecords<PolicesRetireesCsv>()
                    .Where(x => 
                        completed.All(c => c.PolicyNumber != x.PolicyNumber)
                    )
                    .Where(x => completed.All(y => x.ContractorLink != y.Rfc))
                    // .Take(10)
                    .ToList();
        
                ctx.Status("Downloading documents...");
                await Task.Delay(1000, cancellationToken);
        
                var total = records.Count;
                var currentCount = 0;
                var text = "Download documents";
                await Parallel.ForEachAsync(records, cancellationToken, async (data, token) =>
                {
                    var downloadable = data.GetDownloadable();
                    if (downloadable is null)
                    {
                        if (unableToProcess.All(x => x != new UnableToProcess(data.PolicyNumber, data.ContractorLink, "Unknown", "No downloadable link found")))
                        {
                            unableToProcess.Add(new UnableToProcess(data.PolicyNumber, data.ContractorLink, "Unknown", "No downloadable link found"));
                        }
                        updateCounter(ctx, text,ref currentCount, ref total);
                        return;
                    }
                    
                    var (link, extension) = downloadable.Value;
                    var responseMessage = await httpClient.GetAsync(link, token);
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        if (unableToProcess.All(x => x != new UnableToProcess(data.PolicyNumber, data.ContractorLink, "Unknown",  responseMessage.ReasonPhrase!)))
                        {
                            unableToProcess.Add(new UnableToProcess(data.PolicyNumber, data.ContractorLink, "Unknown", responseMessage.ReasonPhrase!));
                        }
                        updateCounter(ctx, text, ref currentCount, ref total);
                        return;
                    }
                    
                    await using var stream = await responseMessage.Content.ReadAsStreamAsync(token);
                    var tempFile = $"{data.PolicyNumber.Replace('/', '_').Replace(' ', '_')}_{data.ContractorLink.Replace('/', '_').Replace(' ', '_')}{extension}";
                    var dir = Path.Combine(saveDirectoryPath, "files", tempFile);
                    await using var fileStream = new FileStream(dir, FileMode.Create, FileAccess.Write, FileShare.None);
                    await stream.CopyToAsync(fileStream, token);
        
                    if (processed.All(x => x != new Processed(data.PolicyNumber, data.ContractorLink, dir)))
                    {
                        processed.Add(new Processed(data.PolicyNumber, data.ContractorLink, dir));
                    }
                    
                    updateCounter(ctx, "Download documents", ref currentCount, ref total);
                });
        
                ctx.Status($"Download documents {total} of {total}");
                await Task.Delay(1000, cancellationToken);
                ctx.Status("Processing documents...");
                await Task.Delay(1000, cancellationToken);
                
                total = processed.Count;
                currentCount = 0;
                text = "Processed document";
                
                await Parallel.ForEachAsync(processed, cancellationToken, async (data, token) =>
                {
                    try
                    {
                        var result = await processAi.ProcessPaycheckFile(data.FileName, token);
                    
                        var totalPercepciones = result.Perceptions.Where(x => _codesToCalculate.Contains(x.Concept))
                            .Sum(p => p.Amount);
                    
                        var totalDeduction = result.Deductions.Sum(p => p.Amount);
                    
                        var r = (totalPercepciones - totalDeduction) * 0.30;
                        var ccl = r - 9_852.00;
        
                        if (completed.All(x =>
                                x != new Completed(data.PolicyNumber, result.RFC, result.Name, $"${ccl:N2}", ccl < 0)))
                        {
                            completed.Add(new Completed(data.PolicyNumber, result.RFC, result.Name,  $"${ccl:N2}", ccl < 0));
                        }
                        
                    }
                    catch (Exception e)
                    {
                        if (unableToProcess.All(x =>
                                x != new UnableToProcess(data.PolicyNumber, data.Rfc, "Unknown", e.Message)))
                        {
                            unableToProcess.Add(new UnableToProcess(data.PolicyNumber, data.Rfc, "Unknown", e.Message));
                        }
                    }
                    updateCounter(ctx, text, ref currentCount, ref total);
                });
                
            });
        
        await File.WriteAllTextAsync(Path.Combine(saveDirectoryPath, "completed.json"), JsonSerializer.Serialize(completed), encoding: new UnicodeEncoding(), cancellationToken);
        // await File.WriteAllTextAsync(Path.Combine(saveDirectoryPath, "processed.json"), JsonSerializer.Serialize(processed), encoding: new UnicodeEncoding(), cancellationToken);
        await File.WriteAllTextAsync(Path.Combine(saveDirectoryPath, "processed-failed.json"), JsonSerializer.Serialize(unableToProcess), encoding: new UnicodeEncoding(), cancellationToken);
        
        AnsiConsole.Record();
        
        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Gray42);
        
        table.AddColumn("Policy Number");
        table.AddColumn("RFC");
        table.AddColumn("Name");
        table.AddColumn("Liquidity Capacity");
        table.AddColumn("Is a potential issue?");

        foreach (var (policyNumber, rfc, name, liquidity, isRisk) in completed)
        {
            try
            {
                table.AddRow(policyNumber ?? "-", rfc ?? "-",name ?? "Unknown",liquidity ?? "??", isRisk ? "[red bold]YES[/]":"[green bold]NO[/]");
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        var panelProcessed = new Panel(table)
            .Header("Processed Records", Justify.Left)
            .BorderColor(Color.Green);
        
        AnsiConsole.Write(panelProcessed);
        
        
        var tableNoProcessed = new Table()
            .RoundedBorder()
            .BorderColor(Color.Gray42);
        tableNoProcessed.AddColumn("Policy Number");
        tableNoProcessed.AddColumn("RFC");
        tableNoProcessed.AddColumn("Name");
        tableNoProcessed.AddColumn("Reason");
        
        foreach (var (policyNumber,rfc, name, reason) in unableToProcess)
        {
            try
            { 
                tableNoProcessed.AddRow(policyNumber ?? "-", rfc, name ?? "Unknown", reason.Replace("[","(").Replace("]",")"));

            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        var panelNotProcessed = new Panel(tableNoProcessed)
            .Header("Records not able to be processed", Justify.Left)
            .BorderColor(Color.Red);
        
        
        AnsiConsole.Write(panelNotProcessed);
        
        var result = AnsiConsole.ExportHtml();
        await File.WriteAllTextAsync(saveDirectory, result, encoding: new UnicodeEncoding(), cancellationToken);
        
        return 0;
    }

    private void updateCounter(StatusContext ctx, string text, ref int currentCount, ref int total)
    {
        lock (expression)
        {
            currentCount++;
            ctx.Status($"{text} {currentCount} of {total}");
            
        }
    }
}