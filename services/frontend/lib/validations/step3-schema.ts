import { z } from 'zod';
import {
  ESTADOS_REPUBLICA,
  PERIODO_PAGO_OPTIONS,
  TIPO_COTIZACION_OPTIONS,
} from '@/types/step3-form';
import type { PeriodoPago, TipoCotizacion } from '@/types/step3-form';

// Day validation (1-31)
const daySchema = z.string()
  .min(1, 'El día es requerido')
  .regex(/^([1-9]|[12][0-9]|3[01])$/, 'Día inválido (1-31)');

// Month validation (1-12)
const monthSchema = z.string()
  .min(1, 'El mes es requerido')
  .regex(/^([1-9]|1[0-2])$/, 'Mes inválido (1-12)');

// Year validation (1900-2099)
const yearSchema = z.string()
  .min(1, 'El año es requerido')
  .regex(/^(19|20)\d{2}$/, 'Año inválido');

export const step3Schema = z.object({
  diaFirma: daySchema,
  mesFirma: monthSchema,
  anoFirma: yearSchema,
  estadoVenta: z.enum(ESTADOS_REPUBLICA as [string, ...string[]], {
    required_error: 'El estado es requerido',
    invalid_type_error: 'Seleccione un estado válido',
  }),
  municipioVenta: z.string()
    .min(1, 'El municipio o alcaldía es requerido'),

  // Policy Configuration
  periodoPago: z.custom<PeriodoPago>(
    (val) => PERIODO_PAGO_OPTIONS.includes(val as PeriodoPago),
    'Periodo de pago es requerido'
  ),
  tipoCotizacion: z.custom<TipoCotizacion>(
    (val) => TIPO_COTIZACION_OPTIONS.includes(val as TipoCotizacion),
    'Tipo de cotización es requerido'
  ),
  sumaAseguradaCotizada: z.string().min(1, 'Suma asegurada es requerida'),
  primaSeguroRiesgoAnual: z.string().min(1, 'Prima del seguro/riesgo es requerida'),
  primaAhorroExcedenteAnual: z.string().min(1, 'Prima de ahorro o excedente es requerida'),
  aseguradoTieneOtrasPolizas: z.enum(['Si', 'No'], {
    message: 'Este campo es requerido',
  }),

  // Payment Details
  numeroTarjeta: z.string().min(1, 'Número de tarjeta es requerido'),
  fechaVencimientoTarjeta: z.string().min(1, 'Fecha de vencimiento es requerida'),
  clabe: z
    .string()
    .regex(/^\d{18}$/, 'La CLABE debe tener 18 dígitos')
    .min(18, 'La CLABE debe tener 18 dígitos'),
  banco: z.string().min(1, 'Banco es requerido'),
  fechaProximoCobro: z.string().min(1, 'Fecha de próximo cobro es requerida'),

  // Beneficiaries
  beneficiarios: z
    .array(
      z.object({
        nombresBeneficiario: z.string().min(1, 'Nombre es requerido'),
        primerApellidoBeneficiario: z.string().min(1, 'Primer apellido es requerido'),
        segundoApellidoBeneficiario: z.string().min(1, 'Segundo apellido es requerido'),
        parentescoBeneficiario: z.string().min(1, 'Parentesco es requerido'),
        diaNacimientoBeneficiario: z.string().min(1, 'Día es requerido'),
        mesNacimientoBeneficiario: z.string().min(1, 'Mes es requerido'),
        anoNacimientoBeneficiario: z.string().min(1, 'Año es requerido'),
        porcentajeSumaAsegurada: z.string().min(1, 'Porcentaje es requerido'),
      })
    )
    .min(1, 'Debe agregar al menos un beneficiario'),
});

export type Step3FormData = z.infer<typeof step3Schema>;
