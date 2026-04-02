import { z } from 'zod';

// Helper to validate file
const fileSchema = z.custom<File>((val) => val instanceof File, {
  message: 'Expected a file',
});

// Schema for individual file validation
const createFileFieldSchema = (
  required: boolean,
  maxSize: number,
  acceptedTypes: string[]
) => {
  const baseSchema = fileSchema
    .refine(
      (file) => file.size <= maxSize,
      {
        message: `File size must be less than ${maxSize / (1024 * 1024)}MB`,
      }
    )
    .refine(
      (file) => {
        const extension = `.${file.name.split('.').pop()?.toLowerCase()}`;
        return acceptedTypes.some((type) => type.toLowerCase() === extension);
      },
      {
        message: `Invalid file type. Accepted: ${acceptedTypes.join(', ')}`,
      }
    );

  if (required) {
    return baseSchema;
  }

  return baseSchema.optional().nullable();
};

// Main form schema for step 1 (file uploads)
export const step1Schema = z.object({
  paycheck: createFileFieldSchema(
    true,
    10 * 1024 * 1024, // 10MB
    ['.pdf', '.jpg', '.jpeg', '.png']
  ),
  idFront: createFileFieldSchema(
    true,
    5 * 1024 * 1024, // 5MB
    ['.jpg', '.jpeg', '.png']
  ),
  idBack: createFileFieldSchema(
    true,
    5 * 1024 * 1024, // 5MB
    ['.jpg', '.jpeg', '.png']
  ),
  letter: createFileFieldSchema(
    false,
    10 * 1024 * 1024, // 10MB
    ['.pdf', '.jpg', '.jpeg', '.png']
  ),
  proofOfAddress: createFileFieldSchema(
    false,
    10 * 1024 * 1024, // 10MB
    ['.pdf', '.jpg', '.jpeg', '.png']
  ),
  picture: createFileFieldSchema(
    true,
    5 * 1024 * 1024, // 5MB
    ['.jpg', '.jpeg', '.png']
  ),
});

export type Step1FormData = z.infer<typeof step1Schema>;

// Schema for extracted information (populated after processing)
export const extractedInfoSchema = z.object({
  firstName: z.string().nullable(),
  lastName: z.string().nullable(),
  idNumber: z.string().nullable(),
  registrationNumber: z.string().nullable(),
});

export type ExtractedInfoData = z.infer<typeof extractedInfoSchema>;

// Full form schema (will be extended for future steps)
export const fullFormSchema = z.object({
  step1: step1Schema,
  extractedInfo: extractedInfoSchema,
});

export type FullFormData = z.infer<typeof fullFormSchema>;
