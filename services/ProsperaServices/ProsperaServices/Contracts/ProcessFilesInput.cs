using ProsperaServices.Enums;

namespace ProsperaServices.Contracts;

public record ProcessFilesInput(IFormFile Talone, IFormFile IdFront, IFormFile IdBack, IFormFile? InstructionLetter);
public record ProcessPolicyInput(IFormFile PolicyDocument);
public record ProcessFileInput(IFormFile File, string sessionToken);
public record ProcessAdditionalFileInput(IFormFile File, string sessionToken, DocumentTypes documentType);
