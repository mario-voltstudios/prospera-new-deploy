using System.Text.Json;
using System.Text.Json.Serialization;
using AIProcess;
using AIProcess.Models;
using Microsoft.Extensions.Caching.Hybrid;
using ProsperaServices.Contracts;
using ProsperaServices.Interfaces;
using ProsperaServices.Interfaces.IoC;
using ProsperaServices.Models;
using ProsperaServices.Modes;

namespace ProsperaServices.ApplicationServices;

public class ProcessFilesService(
    ProcessAI processAi,
    IEncryptionService encryptionService,
    IHttpContextAccessor contextAccessor) : ITransientDependency
{
    private static readonly JsonSerializerOptions CompactJsonOptions = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    public Result<object> CreateSession()
    {
        var sessionToken =
            encryptionService.Encrypt(JsonSerializer.Serialize(SessionInfo.CreateSessionInfo(), CompactJsonOptions));
        return new
        {
            sessionToken
        };
    }

    public async Task<Result<IdResult>> ProcessIdFrontFile(ProcessFileInput input, HybridCache cache,
        CancellationToken cancellationToken = default)
    {
        var hasError = SessionInfo.GetSessionInfo(encryptionService, input.sessionToken, out var sessionResult);

        var (sessionInfo, error) = sessionResult;
        if (hasError)
        {
            contextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return error!;
        }

        var id = sessionInfo!.SessionId;


        var dictionay = await cache.GetOrCreateAsync(id,
            cancel => ValueTask.FromResult(new Dictionary<string, string>()), new HybridCacheEntryOptions
            {
                Expiration = sessionInfo.TimeToExpireInMinutes,
            }, cancellationToken: cancellationToken);

        var key = $"id-front:{input.File.Name}";

        var resultString = dictionay.GetValueOrDefault(key);

        IdResult result;

        if (resultString is null)
        {
            var currentKey = dictionay.Keys.FirstOrDefault(x => x.StartsWith("id-front:"));

            if (!string.IsNullOrEmpty(currentKey))
            {
                dictionay.Remove(currentKey);
            }

            result = JsonSerializer.Deserialize<IdResult>("""
                                                          {
                                                            "nombre": "JESSICA ELIZABETH",
                                                            "apellido_paterno": "AGUILAR",
                                                            "apellido_materno": "REYES",
                                                            "fecha_nacimiento": "1980-08-21T00:00:00Z",
                                                            "domicilio": {
                                                              "calle": "TAMAULIPAS",
                                                              "numero_exterior": "405",
                                                              "numero_interior": "",
                                                              "colonia": "SANTA MARIA TULPETLAC",
                                                              "municipio": "ECATEPEC DE MORELOS",
                                                              "estado": "MEX.",
                                                              "codigo_postal": "55400",
                                                              "esta_completado": true
                                                            },
                                                            "curp": "AURJ800821MDFGYS07",
                                                            "sexo": "M"
                                                          }
                                                          """)!;
            dictionay.Add(key, JsonSerializer.Serialize(result));

            // var file = input.File.OpenReadStream();
            // var result = await processAi.ProcessIdFile(file, input.File.ContentType, cancellationToken);
            // file.Seek(0, SeekOrigin.Begin);
            // return result;
        }
        else
        {
            result = JsonSerializer.Deserialize<IdResult>(resultString)!;
        }

        await cache.SetAsync(id, dictionay, new HybridCacheEntryOptions
        {
            Expiration = sessionInfo.TimeToExpireInMinutes,
        }, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<Result<IdResultBack>> ProcessIdBackFile(ProcessFileInput input, HybridCache cache,
        CancellationToken cancellationToken = default)
    {
        var isValid = SessionInfo.GetSessionInfo(encryptionService, input.sessionToken, out var sessionResult);
        var (sessionInfo, error) = sessionResult;
        if (isValid)
        {
            contextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return error!;
        }

        var id = sessionInfo!.SessionId;

        var dictionay = await cache.GetOrCreateAsync(id,
            cancel => ValueTask.FromResult(new Dictionary<string, string>()), new HybridCacheEntryOptions
            {
                Expiration = sessionInfo.TimeToExpireInMinutes,
            }, cancellationToken: cancellationToken);

        var key = $"id-back:{input.File.Name}";

        var resultString = dictionay.GetValueOrDefault(key);

        IdResultBack result;

        if (resultString is null)
        {
            var currentKey = dictionay.Keys.FirstOrDefault(x => x.StartsWith("id-front:"));

            if (!string.IsNullOrEmpty(currentKey))
            {
                dictionay.Remove(currentKey);
            }

            result = JsonSerializer.Deserialize<IdResultBack>("""
                                                              {
                                                                "idmex": "00012012031230"
                                                              }
                                                              """)!;
            dictionay.Add(key, JsonSerializer.Serialize(result));

            // var file = input.File.OpenReadStream();
            // result = await processAi.ProcessIdFileBck(file, input.File.ContentType, cancellationToken);
            // file.Seek(0, SeekOrigin.Begin);
        }
        else
        {
            result = JsonSerializer.Deserialize<IdResultBack>(resultString)!;
        }

        await cache.SetAsync(id, dictionay, new HybridCacheEntryOptions
        {
            Expiration = sessionInfo.TimeToExpireInMinutes,
        }, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<Result<PaycheckResult>> ProcessPaycheckFile(ProcessFileInput input, HybridCache cache,
        CancellationToken cancellationToken = default)
    {
        var isValid = SessionInfo.GetSessionInfo(encryptionService, input.sessionToken, out var sessionResult);
        var (sessionInfo, error) = sessionResult;
        if (isValid)
        {
            contextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return error!;
        }

        var id = sessionInfo!.SessionId;

        var dictionay = await cache.GetOrCreateAsync(id,
            cancel => ValueTask.FromResult(new Dictionary<string, string>()), new HybridCacheEntryOptions
            {
                Expiration = sessionInfo.TimeToExpireInMinutes,
            }, cancellationToken: cancellationToken);

        var key = $"paycheck:{input.File.Name}";

        var resultString = dictionay.GetValueOrDefault(key);

        PaycheckResult result;


        if (resultString is null)
        {
            result = JsonSerializer.Deserialize<PaycheckResult>("""
                                                                {
                                                                  "matricula": "98152413",
                                                                  "RFC": "AURJ800821557",
                                                                  "nombre": "JESSICA ELIZABETH AGUILAR REYES",
                                                                  "curp": null,
                                                                  "tipo_de_contratacion": 2,
                                                                  "clave_est_org": "15UA68210A",
                                                                  "type": 15,
                                                                  "percepciones": [
                                                                    {
                                                                      "concepto": "002",
                                                                      "descripcion": "Sueldo Base Fijo",
                                                                      "importe": 2948.7
                                                                    },
                                                                    {
                                                                      "concepto": "011",
                                                                      "descripcion": "Ayuda Renta Cláusula 63 Bis Inc b",
                                                                      "importe": 2422.36
                                                                    },
                                                                    {
                                                                      "concepto": "020",
                                                                      "descripcion": "Ayuda Renta Cláusula 63 Bis Inc a",
                                                                      "importe": 250
                                                                    },
                                                                    {
                                                                      "concepto": "022",
                                                                      "descripcion": "Ayuda Renta Cláusula 63 Bis Inc c",
                                                                      "importe": 1700.83
                                                                    },
                                                                    {
                                                                      "concepto": "026",
                                                                      "descripcion": "Pasajes Fijos",
                                                                      "importe": 326.67
                                                                    },
                                                                    {
                                                                      "concepto": "033",
                                                                      "descripcion": "Estímulo por Puntualidad",
                                                                      "importe": 716.14
                                                                    },
                                                                    {
                                                                      "concepto": "047",
                                                                      "descripcion": "Anticipo de Aguinaldo en Enero",
                                                                      "importe": 5371.05
                                                                    },
                                                                    {
                                                                      "concepto": "050",
                                                                      "descripcion": "Ayuda para Despensa",
                                                                      "importe": 200
                                                                    },
                                                                    {
                                                                      "concepto": "072",
                                                                      "descripcion": "Ayuda para Libros",
                                                                      "importe": 268.55
                                                                    },
                                                                    {
                                                                      "concepto": "083",
                                                                      "descripcion": "Sobresueldo Investigación y Docencia",
                                                                      "importe": 137.61
                                                                    }
                                                                  ],
                                                                  "deducciones": [
                                                                    {
                                                                      "concepto": "109",
                                                                      "descripcion": "Prima del Seg Daños Vivienda",
                                                                      "importe": 15
                                                                    },
                                                                    {
                                                                      "concepto": "111",
                                                                      "descripcion": "Aport Complementaria Afore",
                                                                      "importe": 1390.62
                                                                    },
                                                                    {
                                                                      "concepto": "112",
                                                                      "descripcion": "Fondo Ayuda Sindical por Defunción",
                                                                      "importe": 55.31
                                                                    },
                                                                    {
                                                                      "concepto": "151",
                                                                      "descripcion": "ISR",
                                                                      "importe": 174.12
                                                                    },
                                                                    {
                                                                      "concepto": "154",
                                                                      "descripcion": "Descuento Crédito INFONAVIT",
                                                                      "importe": 1230.45
                                                                    },
                                                                    {
                                                                      "concepto": "171",
                                                                      "descripcion": "Licencias sin sueldo hasta 3 días",
                                                                      "importe": 1058.38
                                                                    },
                                                                    {
                                                                      "concepto": "172",
                                                                      "descripcion": "Faltas Injustificadas",
                                                                      "importe": 358.07
                                                                    },
                                                                    {
                                                                      "concepto": "173",
                                                                      "descripcion": "Pases de Salida",
                                                                      "importe": 244.04
                                                                    },
                                                                    {
                                                                      "concepto": "180",
                                                                      "descripcion": "Cuota Sindical",
                                                                      "importe": 107.42
                                                                    },
                                                                    {
                                                                      "concepto": "190",
                                                                      "descripcion": "Caja de ahorro préstamo",
                                                                      "importe": 674.5
                                                                    },
                                                                    {
                                                                      "concepto": "192",
                                                                      "descripcion": "Caja de Ahorro Ahorro",
                                                                      "importe": 1430
                                                                    },
                                                                    {
                                                                      "concepto": "195",
                                                                      "descripcion": "Seguro Individual Voluntario Vida",
                                                                      "importe": 300
                                                                    }
                                                                  ],
                                                                  "observaciones": [
                                                                    {
                                                                      "concepto": "154",
                                                                      "importe": 1230.45,
                                                                      "vencimento": "2054010",
                                                                      "unidades": "24",
                                                                      "num_control": "20120809",
                                                                      "observaciones": "1512451526"
                                                                    },
                                                                    {
                                                                      "concepto": "171",
                                                                      "importe": 1058.38,
                                                                      "vencimento": "2026001",
                                                                      "unidades": "2",
                                                                      "num_control": "",
                                                                      "observaciones": "04122025-05122025"
                                                                    },
                                                                    {
                                                                      "concepto": "172",
                                                                      "importe": 358.07,
                                                                      "vencimento": "2026001",
                                                                      "unidades": "7",
                                                                      "num_control": "",
                                                                      "observaciones": "12122025-12122025"
                                                                    },
                                                                    {
                                                                      "concepto": "173",
                                                                      "importe": 244.04,
                                                                      "vencimento": "2026001",
                                                                      "unidades": "4",
                                                                      "num_control": "",
                                                                      "observaciones": "02122025-02122025"
                                                                    },
                                                                    {
                                                                      "concepto": "190",
                                                                      "importe": 674.5,
                                                                      "vencimento": "2037005",
                                                                      "unidades": "270",
                                                                      "num_control": "48",
                                                                      "observaciones": "1,349.00 180,766.00"
                                                                    },
                                                                    {
                                                                      "concepto": "192",
                                                                      "importe": 1430,
                                                                      "vencimento": "2030001",
                                                                      "unidades": "98",
                                                                      "num_control": "48",
                                                                      "observaciones": ""
                                                                    },
                                                                    {
                                                                      "concepto": "195",
                                                                      "importe": 300,
                                                                      "vencimento": "2999099",
                                                                      "unidades": "",
                                                                      "num_control": "160821",
                                                                      "observaciones": "461340374"
                                                                    },
                                                                    {
                                                                      "concepto": "026",
                                                                      "importe": 326.67,
                                                                      "vencimento": "2026001",
                                                                      "unidades": "",
                                                                      "num_control": "8001",
                                                                      "observaciones": "PA2515/02535"
                                                                    }
                                                                  ],
                                                                  "contains_gnp_policy": false
                                                                }
                                                                """)!;

            // var file = input.File.OpenReadStream();
            // var result = await processAi.ProcessPaycheckFile(file, input.File.ContentType, cancellationToken);
            // file.Seek(0, SeekOrigin.Begin);

            dictionay.Add(key, JsonSerializer.Serialize(result));
        }
        else
        {
            result = JsonSerializer.Deserialize<PaycheckResult>(resultString)!;
        }

        return result;
    }

    public async Task<Result<LetterInformationResult>> ProcessLetterFile(ProcessFileInput input, HybridCache cache,
        CancellationToken cancellationToken = default)
    {
        var isValid = SessionInfo.GetSessionInfo(encryptionService, input.sessionToken, out var sessionResult);
        var (sessionInfo, error) = sessionResult;
        if (isValid)
        {
            contextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return error!;
        }

        var id = sessionInfo!.SessionId;

        var dictionay = await cache.GetOrCreateAsync(id,
            cancel => ValueTask.FromResult(new Dictionary<string, string>()), new HybridCacheEntryOptions
            {
                Expiration = sessionInfo.TimeToExpireInMinutes,
            }, cancellationToken: cancellationToken);

        var key = $"LetterFile:{input.File.Name}";

        {
            var file = input.File.OpenReadStream();
            var result = await processAi.ProcessInstructionLetter(file, input.File.ContentType, cancellationToken);
            file.Seek(0, SeekOrigin.Begin);
            return result;
        }
    }

    public async Task<Result<object>> ProcessAddressProof(ProcessFileInput input, HybridCache cache,
        CancellationToken cancellationToken = default)
    {
        return new { };
    }

    public async Task<Result<object>> UploadPhoto(ProcessFileInput input, HybridCache cache,
        CancellationToken cancellationToken = default)
    {
        contextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status202Accepted;
        return new { };
    }

    public async Task<Result<object>> UploadAdditionalDocuments(ProcessAdditionalFileInput input, HybridCache cache,
        CancellationToken cancellationToken = default)
    {
        var isValid = SessionInfo.GetSessionInfo(encryptionService, input.sessionToken, out var sessionResult);
        var (sessionInfo, error) = sessionResult;
        if (isValid)
        {
            contextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return error!;
        }

        var id = sessionInfo!.SessionId;


        var dictionay = await cache.GetOrCreateAsync(id,
            cancel => ValueTask.FromResult(new Dictionary<string, string>()), new HybridCacheEntryOptions
            {
                Expiration = sessionInfo.TimeToExpireInMinutes,
            }, cancellationToken: cancellationToken);


        contextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status202Accepted;

        return new { };
    }

    public async Task ValidateDocuments(HybridCache cache, CancellationToken cancellationToken = default)
    {
        // cache.
    }

    public async Task<Result<object>> Execute(ProcessFilesInput input, HybridCache cache,
        CancellationToken cancellationToken = default)
    {
        var task1 = processAi.ProcessIdFile(input.IdFront.OpenReadStream(), input.IdFront.ContentType,
            cancellationToken);
        var task2 = processAi.ProcessIdFileBck(input.IdBack.OpenReadStream(), input.IdBack.ContentType,
            cancellationToken);
        var task3 = processAi.ProcessPaycheckFile(input.Talone.OpenReadStream(), input.Talone.ContentType,
            cancellationToken);

        List<Task> taskTorun =
        [
            task1,
            task2,
            task3
        ];

        Task<LetterInformationResult>? task4 = null;
        if (input.InstructionLetter is not null)
        {
            task4 = processAi.ProcessInstructionLetter(input.InstructionLetter.OpenReadStream(),
                input.InstructionLetter.ContentType, cancellationToken);
            taskTorun.Add(task4);
        }

        await Task.WhenAll(taskTorun);

        return new
        {
            // IdFront = await task1,
            // IdBack = await task2,
            // Talones = await task3
            letter = task4?.Result
        };
    }

    public async Task<object> ExecutePoliceProcess(ProcessPolicyInput input, HybridCache cache,
        CancellationToken cancellationToken = default)
    {
        var task = await processAi.ProcessPoliceFile(input.PolicyDocument.OpenReadStream(),
            input.PolicyDocument.ContentType, cancellationToken);
        return new
        {
            PolicyData = task
        };
    }

    public async Task<Result<object>> SearchLocation(string query, CancellationToken cancellationToken = default)
    {
        var resutl = await processAi.SearchLocation(query, cancellationToken);

        return new
        {
            results = resutl.Select(x => new { x.Code })
        };
    }
}