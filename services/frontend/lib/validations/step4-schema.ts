import { z } from 'zod';

// RFC validation: 12 or 13 characters (AAAA######XXX or AAA######XXX)
const rfcRegex = /^[A-ZÑ&]{3,4}\d{6}[A-Z0-9]{3}$/;

// Email validation
const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

export const step4Schema = z.object({
  claveAgente: z.string()
    .min(1, 'La clave de agente es requerida')
    .max(50, 'La clave de agente no puede exceder 50 caracteres'),
  
  tieneCedulaVigente: z.enum(['si', 'no'], {
    required_error: 'Debes seleccionar una opción',
  }),
  
  rfc: z.string()
    .min(1, 'El RFC es requerido')
    .regex(rfcRegex, 'El RFC debe tener el formato correcto (ej: ABCD123456XXX)'),
  
  correo: z.string()
    .min(1, 'El correo es requerido')
    .regex(emailRegex, 'El correo debe tener un formato válido'),
});

export type Step4Schema = z.infer<typeof step4Schema>;
