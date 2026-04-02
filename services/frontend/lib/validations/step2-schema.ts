import { z } from 'zod';
import type {
  Gender,
  RegimenFiscal,
  TipoIdentificacion,
  Profesion,
  Dependencia,
  MetodoPago,
  ClaveDelegacional,
  TipoContrato,
  UbicacionSubdelegacion,
  Alcaldia,
  Estado,
  ProfesionAsegurado,
} from '@/types/step2-form';
import {
  CLAVE_DELEGACIONAL_OPTIONS,
  TIPO_CONTRATO_OPTIONS,
  UBICACION_SUBDELEGACION_OPTIONS,
  ALCALDIA_OPTIONS,
  ESTADO_OPTIONS,
  PROFESION_ASEGURADO_OPTIONS,
  TIPO_IDENTIFICACION_OPTIONS,
  GENDER_OPTIONS,
} from '@/types/step2-form';

// Step 2: Hire Information Schema
export const step2Schema = z.object({
  // Personal Information
  nombresContratante: z.string().min(1, 'Nombres is required'),
  primerApellidoContratante: z.string().min(1, 'Primer Apellido is required'),
  segundoApellidoContratante: z.string().min(1, 'Segundo Apellido is required'),
  diaNacimientoContratante: z
    .string()
    .min(1, 'Day is required')
    .regex(/^(0?[1-9]|[12][0-9]|3[01])$/, 'Invalid day (1-31)'),
  mesNacimientoContratante: z
    .string()
    .min(1, 'Month is required')
    .regex(/^(0?[1-9]|1[0-2])$/, 'Invalid month (1-12)'),
  anioNacimientoContratante: z
    .string()
    .min(4, 'Year is required')
    .regex(/^(19|20)\d{2}$/, 'Invalid year'),
  generoContratante: z.custom<Gender>((val) => {
    return val === 'Masculino' || val === 'Femenino';
  }, 'Gender is required'),
  rfcContratante: z
    .string()
    .min(1, 'RFC is required')
    .regex(/^[A-ZÑ&]{3,4}\d{6}[A-Z0-9]{3}$/, 'Invalid RFC format'),
  entidadFederativa: z.string().optional(),
  nacionalidad: z.string().min(1, 'Nacionalidad is required'),
  regimenFiscal: z.custom<RegimenFiscal>((val) => {
    return typeof val === 'string' && val.length > 0;
  }, 'Regimen Fiscal is required'),
  tipoIdentificacion: z.custom<TipoIdentificacion>((val) => {
    return typeof val === 'string' && val.length > 0;
  }, 'Tipo de Identificacion is required'),
  organismoEmiteIdentificacion: z.string().min(1, 'Organismo Emisor is required'),
  numeroIdentificacion: z.string().min(1, 'Numero de Identificacion is required'),

  // Address
  calleContratante: z.string().min(1, 'Calle is required'),
  numeroExteriorContratante: z.string().min(1, 'Numero Exterior is required'),
  numeroInteriorContratante: z.string().optional(),
  codigoPostalContratante: z
    .string()
    .min(5, 'Codigo Postal is required')
    .regex(/^\d{5}$/, 'Invalid postal code (5 digits)'),
  coloniaContratante: z.string().min(1, 'Colonia is required'),
  estadoContratante: z.string().min(1, 'Estado is required'),
  municipioContratante: z.string().min(1, 'Municipio is required'),
  paisContratante: z.string().min(1, 'Pais is required'),

  // Contact Information
  correoElectronicoContratante: z
    .string()
    .min(1, 'Email is required')
    .email('Invalid email address'),
  telefonoMovilContratante: z
    .string()
    .min(10, 'Phone number is required')
    .regex(/^\d{10}$/, 'Invalid phone number (10 digits)'),

  // Professional Information
  profesionContratante: z.custom<Profesion>((val) => {
    return typeof val === 'string' && val.length > 0;
  }, 'Profesion is required'),
  dependenciaEmpresa: z.custom<Dependencia>((val) => {
    return typeof val === 'string' && val.length > 0;
  }, 'Dependencia/Empresa is required'),
  metodoPago: z.custom<MetodoPago>((val) => {
    return typeof val === 'string' && val.length > 0;
  }, 'Metodo de Pago is required'),

  // IMSS Information
  claveDelegacionalIMSS: z.custom<ClaveDelegacional>(
    (val) => CLAVE_DELEGACIONAL_OPTIONS.includes(val as ClaveDelegacional),
    'Clave Delegacional is required'
  ),
  matriculaIMSS: z.string().min(1, 'Matricula is required'),
  tipoContratoIMSS: z.custom<TipoContrato>(
    (val) => TIPO_CONTRATO_OPTIONS.includes(val as TipoContrato),
    'Tipo de Contrato is required'
  ),
  nombreUbicacion: z.string().min(1, 'Nombre de Ubicacion is required'),
  ubicacionSubdelegacion: z.custom<UbicacionSubdelegacion>(
    (val) => UBICACION_SUBDELEGACION_OPTIONS.includes(val as UbicacionSubdelegacion),
    'Ubicacion Subdelegacion is required'
  ),

  // GOB CDMX Information
  numeroEmpleadoCDMX: z.string().min(1, 'Numero del Empleado is required'),
  alcaldiaCDMX: z.custom<Alcaldia>(
    (val) => ALCALDIA_OPTIONS.includes(val as Alcaldia),
    'Alcaldia is required'
  ),
  edificioUbicacionCDMX: z.string().min(1, 'Edificio o Ubicacion is required'),

  // SEP Information
  centroTrabajoSEP: z.string().min(1, 'Centro de trabajo is required'),
  primerosDosDigitosCCT: z.string().min(2, 'Primeros 2 digitos del CCT is required').max(2, 'Solo 2 digitos'),
  cartaAutorizacionSEP: z.string().optional(),

  // ISSEMYM Information
  claveISSEMYM: z.string().min(1, 'Clave ISSEMYM is required'),

  // Insured Person Information
  aseguradoEsContratante: z.enum(['Si', 'No'], {
    message: 'Este campo es requerido',
  }),
  nombresAsegurado: z.string().optional(),
  primerApellidoAsegurado: z.string().optional(),
  segundoApellidoAsegurado: z.string().optional(),
  diaNacimientoAsegurado: z.string().optional(),
  mesNacimientoAsegurado: z.string().optional(),
  anioNacimientoAsegurado: z.string().optional(),
  generoAsegurado: z.custom<Gender>((val) => !val || GENDER_OPTIONS.includes(val as Gender)).optional(),
  rfcAsegurado: z.string().optional(),
  estadoNacimientoAsegurado: z.custom<Estado>((val) => !val || ESTADO_OPTIONS.includes(val as Estado)).optional(),
  nacionalidadAsegurado: z.string().optional(),
  tipoIdentificacionAsegurado: z.custom<TipoIdentificacion>((val) => !val || TIPO_IDENTIFICACION_OPTIONS.includes(val as TipoIdentificacion)).optional(),
  organismoEmiteIdentificacionAsegurado: z.string().optional(),
  numeroIdentificacionAsegurado: z.string().optional(),
  correoElectronicoAsegurado: z.string().email('Email invalido').optional().or(z.literal('')),
  telefonoMovilAsegurado: z.string().regex(/^\d{10}$/, 'Telefono invalido (10 digitos)').optional().or(z.literal('')),
  profesionAsegurado: z.custom<ProfesionAsegurado>((val) => !val || PROFESION_ASEGURADO_OPTIONS.includes(val as ProfesionAsegurado)).optional(),
  direccionAseguradoEsContratante: z.enum(['Si', 'No']).optional(),
  calleAsegurado: z.string().optional(),
  numeroExteriorAsegurado: z.string().optional(),
  numeroInteriorAsegurado: z.string().optional(),
  codigoPostalAsegurado: z.string().optional(),
  coloniaAsegurado: z.string().optional(),
  municipioAsegurado: z.string().optional(),
  estadoAsegurado: z.string().optional(),
  paisAsegurado: z.string().optional(),

  // Health Declaration
  padeceEnfermedad: z.enum(['Si', 'No'], {
    message: 'Este campo es requerido',
  }),
  padecidoCovid19: z.enum(['Si', 'No'], {
    message: 'Este campo es requerido',
  }),
  diasUltimoResultadoPositivo: z.string().optional(),
  requirioAsistenciaRespiratoria: z.enum(['Si', 'No'], {
    message: 'Este campo es requerido',
  }),
  aseguradoFuma: z.enum(['Si', 'No'], {
    message: 'Este campo es requerido',
  }),
}).refine(
  (data) => {
    if (data.aseguradoEsContratante === 'No') {
      return (
        data.nombresAsegurado &&
        data.primerApellidoAsegurado &&
        data.segundoApellidoAsegurado &&
        data.diaNacimientoAsegurado &&
        data.mesNacimientoAsegurado &&
        data.anioNacimientoAsegurado &&
        data.generoAsegurado &&
        data.rfcAsegurado &&
        data.estadoNacimientoAsegurado &&
        data.nacionalidadAsegurado &&
        data.tipoIdentificacionAsegurado &&
        data.organismoEmiteIdentificacionAsegurado &&
        data.numeroIdentificacionAsegurado &&
        data.profesionAsegurado
      );
    }
    return true;
  },
  {
    message: 'Todos los campos del asegurado son requeridos cuando no es el mismo que el contratante',
    path: ['aseguradoEsContratante'],
  }
).refine(
  (data) => {
    if (data.aseguradoEsContratante === 'No' && data.direccionAseguradoEsContratante === 'No') {
      return (
        data.calleAsegurado &&
        data.numeroExteriorAsegurado &&
        data.numeroInteriorAsegurado &&
        data.codigoPostalAsegurado &&
        data.coloniaAsegurado &&
        data.municipioAsegurado &&
        data.estadoAsegurado &&
        data.paisAsegurado
      );
    }
    return true;
  },
  {
    message: 'Todos los campos de direccion del asegurado son requeridos',
    path: ['direccionAseguradoEsContratante'],
  }
);

export type Step2FormData = z.infer<typeof step2Schema>;
