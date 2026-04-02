export type Gender = 'Masculino' | 'Femenino';

export type RegimenFiscal =
  | '605 SUELDOS Y SALARIOS E INGRESOS ASIMILADOS A SALARIOS'
  | '606 ARRENDAMIENTO'
  | '611 INGRESOS POR DIVIDENDOS (SOCIOS Y ACCIONISTAS)'
  | '612 PERSONA FISICA CON ACTIVIDAD EMPRESARIAL'
  | '621 INCORPORACION FISCAL'
  | '622 ACTIVIDADES AGRICOLAS, GANADERAS, SILVICOLAS Y PESQUERAS'
  | '625 REGIMEN DE LAS ACTIVIDADES EMPRESARIALES CON INGRESOS A TRAVES DE PLATAFORMAS TECNOLOGICAS'
  | '626 REGIMEN SIMPLIFICADO DE CONFIANZA';

export type TipoIdentificacion =
  | 'INE'
  | 'IFE'
  | 'CEDULA PROFESIONAL'
  | 'PASAPORTE'
  | 'LICENCIA'
  | 'CREDENCIAL DEL TRABAJADOR';

export type Profesion =
  | 'Empleado'
  | 'Maestro o Docente'
  | 'Administrativo'
  | 'Almacen'
  | 'Doctor'
  | 'Enfermera'
  | 'Personal de apoyo medico'
  | 'Chofer'
  | 'Cobrador'
  | 'Colocación y/o mantenimiento de anuncios'
  | 'Electricista'
  | 'Inmersiones submarina (hasta 40 metros)'
  | 'Instalación y/o mantenimiento de antenas'
  | 'Limpiador de cristales y/o chimenea'
  | 'Mensajero en motocicleta'
  | 'Mineros (Sin manejo de explosivos y hasta 2 días a la semana en mina)'
  | 'Químico Radiología'
  | 'Reparador/Instalador de elevadores'
  | 'Policías'
  | 'Aviación (empleado en talleres y pilotos)'
  | 'Barquero (embarcación en aguas tranquilas o poco profundas)'
  | 'Cargador'
  | 'Chef o Cocinero'
  | 'Mudanzas'
  | 'Venta o instalación de aire Acondicionado'
  | 'Azafata (NO ASEGURABLE)'
  | 'Bombero (NO ASEGURABLE)'
  | 'Calderero (Refinería de petróleo) (NO ASEGURABLE)'
  | 'Carcelero (NO ASEGURABLE)'
  | 'Chofer (Vehículos blindados. Traslado de valores) (NO ASEGURABLE)'
  | 'Diputado (NO ASEGURABLE)'
  | 'Gobernador (NO ASEGURABLE)'
  | 'Guardaespaldas (NO ASEGURABLE)'
  | 'Guardia forestal (NO ASEGURABLE)'
  | 'Inmersiones submarina (más de 40 metros) (NO ASEGURABLE)'
  | 'Magistrados /Juez (Federales) (NO ASEGURABLE)'
  | 'Manejo o contacto con explosivos (NO ASEGURABLE)'
  | 'Marina (navegación) (NO ASEGURABLE)'
  | 'Militar (NO ASEGURABLE)'
  | 'Mineros (Manejo de explosivos y/o mayor o igual a 3 días a la semana en mina) (NO ASEGURABLE)'
  | 'Ministerio público (Locales) (NO ASEGURABLE)';

export type Dependencia =
  | 'SEP'
  | 'ISSSTE JUBILADOS'
  | 'ISSSTE'
  | 'ISSEMYM'
  | 'IMSS JUBILADOS'
  | 'IMSS'
  | 'GOBIERNO DEL ESTADO DE MEXICO'
  | 'GOB CDMX'
  | 'EMPRESA PRIVADA'
  | 'DIF MUNICIPAL TOLUCA'
  | 'UAQ'
  | 'GUARDIA NACIONAL Y SERVIDOR PUBLICO ARMADO'
  | 'OTRA DEPENDENCIA SERVIDOR PUBLICO';

export type MetodoPago =
  | 'Descuento por Nomina'
  | 'Tarjeta de Credito'
  | 'Tarjeta de Debito'
  | 'Cuenta CLABE (OJO en esta modalidad no hay mas de 3 reintentos)';

export type ClaveDelegacional = '15' | '16' | '18' | '23' | '39';

export type TipoContrato = '00' | '01' | '02' | '04' | '07' | '08' | '09' | '10' | '11';

export type UbicacionSubdelegacion = 'Si' | 'No';

export type Alcaldia =
  | 'Azcapotzalco'
  | 'Cuajimalpa'
  | 'Cuauhtemoc'
  | 'Gustavo A Madero'
  | 'Iztacalco'
  | 'Iztapalapa'
  | 'Miguel Hidalgo'
  | 'Tlahuac'
  | 'Venustiano Carranza';

// Step 2 Form Data Interface
export interface Step2FormData {
  // Personal Information
  nombresContratante: string;
  primerApellidoContratante: string;
  segundoApellidoContratante: string;
  diaNacimientoContratante: string;
  mesNacimientoContratante: string;
  anioNacimientoContratante: string;
  generoContratante: Gender;
  rfcContratante: string;
  entidadFederativa: string;
  nacionalidad: string;
  regimenFiscal: RegimenFiscal;
  tipoIdentificacion: TipoIdentificacion;
  organismoEmiteIdentificacion: string;
  numeroIdentificacion: string;

  // Address
  calleContratante: string;
  numeroExteriorContratante: string;
  numeroInteriorContratante: string;
  codigoPostalContratante: string;
  coloniaContratante: string;
  estadoContratante: string;
  municipioContratante: string;
  paisContratante: string;

  // Contact Information
  correoElectronicoContratante: string;
  telefonoMovilContratante: string;

  // Professional Information
  profesionContratante: Profesion;
  dependenciaEmpresa: Dependencia;
  metodoPago: MetodoPago;

  // IMSS Information
  claveDelegacionalIMSS: ClaveDelegacional;
  matriculaIMSS: string;
  tipoContratoIMSS: TipoContrato;
  nombreUbicacion: string;
  ubicacionSubdelegacion: UbicacionSubdelegacion;

  // GOB CDMX Information
  numeroEmpleadoCDMX: string;
  alcaldiaCDMX: Alcaldia;
  edificioUbicacionCDMX: string;

  // SEP Information
  centroTrabajoSEP: string;
  primerosDosDigitosCCT: string;
  cartaAutorizacionSEP?: string;

  // ISSEMYM Information
  claveISSEMYM: string;

  // Insured Person Information
  aseguradoEsContratante: 'Si' | 'No';
  nombresAsegurado?: string;
  primerApellidoAsegurado?: string;
  segundoApellidoAsegurado?: string;
  diaNacimientoAsegurado?: string;
  mesNacimientoAsegurado?: string;
  anioNacimientoAsegurado?: string;
  generoAsegurado?: Gender;
  rfcAsegurado?: string;
  estadoNacimientoAsegurado?: Estado;
  nacionalidadAsegurado?: string;
  tipoIdentificacionAsegurado?: TipoIdentificacion;
  organismoEmiteIdentificacionAsegurado?: string;
  numeroIdentificacionAsegurado?: string;
  correoElectronicoAsegurado?: string;
  telefonoMovilAsegurado?: string;
  profesionAsegurado?: ProfesionAsegurado;
  direccionAseguradoEsContratante?: 'Si' | 'No';
  calleAsegurado?: string;
  numeroExteriorAsegurado?: string;
  numeroInteriorAsegurado?: string;
  codigoPostalAsegurado?: string;
  coloniaAsegurado?: string;
  municipioAsegurado?: string;
  estadoAsegurado?: string;
  paisAsegurado?: string;

  // Health Declaration
  padeceEnfermedad: 'Si' | 'No';
  padecidoCovid19: 'Si' | 'No';
  diasUltimoResultadoPositivo?: string;
  requirioAsistenciaRespiratoria: 'Si' | 'No';
  aseguradoFuma: 'Si' | 'No';
}

// Options for dropdowns
export const GENDER_OPTIONS: Gender[] = ['Masculino', 'Femenino'];

export const REGIMEN_FISCAL_OPTIONS: RegimenFiscal[] = [
  '605 SUELDOS Y SALARIOS E INGRESOS ASIMILADOS A SALARIOS',
  '606 ARRENDAMIENTO',
  '611 INGRESOS POR DIVIDENDOS (SOCIOS Y ACCIONISTAS)',
  '612 PERSONA FISICA CON ACTIVIDAD EMPRESARIAL',
  '621 INCORPORACION FISCAL',
  '622 ACTIVIDADES AGRICOLAS, GANADERAS, SILVICOLAS Y PESQUERAS',
  '625 REGIMEN DE LAS ACTIVIDADES EMPRESARIALES CON INGRESOS A TRAVES DE PLATAFORMAS TECNOLOGICAS',
  '626 REGIMEN SIMPLIFICADO DE CONFIANZA',
];

export const TIPO_IDENTIFICACION_OPTIONS: TipoIdentificacion[] = [
  'INE',
  'IFE',
  'CEDULA PROFESIONAL',
  'PASAPORTE',
  'LICENCIA',
  'CREDENCIAL DEL TRABAJADOR',
];

export const PROFESION_OPTIONS: Profesion[] = [
  'Empleado',
  'Maestro o Docente',
  'Administrativo',
  'Almacen',
  'Doctor',
  'Enfermera',
  'Personal de apoyo medico',
  'Chofer',
  'Cobrador',
  'Colocación y/o mantenimiento de anuncios',
  'Electricista',
  'Inmersiones submarina (hasta 40 metros)',
  'Instalación y/o mantenimiento de antenas',
  'Limpiador de cristales y/o chimenea',
  'Mensajero en motocicleta',
  'Mineros (Sin manejo de explosivos y hasta 2 días a la semana en mina)',
  'Químico Radiología',
  'Reparador/Instalador de elevadores',
  'Policías',
  'Aviación (empleado en talleres y pilotos)',
  'Barquero (embarcación en aguas tranquilas o poco profundas)',
  'Cargador',
  'Chef o Cocinero',
  'Mudanzas',
  'Venta o instalación de aire Acondicionado',
  'Azafata (NO ASEGURABLE)',
  'Bombero (NO ASEGURABLE)',
  'Calderero (Refinería de petróleo) (NO ASEGURABLE)',
  'Carcelero (NO ASEGURABLE)',
  'Chofer (Vehículos blindados. Traslado de valores) (NO ASEGURABLE)',
  'Diputado (NO ASEGURABLE)',
  'Gobernador (NO ASEGURABLE)',
  'Guardaespaldas (NO ASEGURABLE)',
  'Guardia forestal (NO ASEGURABLE)',
  'Inmersiones submarina (más de 40 metros) (NO ASEGURABLE)',
  'Magistrados /Juez (Federales) (NO ASEGURABLE)',
  'Manejo o contacto con explosivos (NO ASEGURABLE)',
  'Marina (navegación) (NO ASEGURABLE)',
  'Militar (NO ASEGURABLE)',
  'Mineros (Manejo de explosivos y/o mayor o igual a 3 días a la semana en mina) (NO ASEGURABLE)',
  'Ministerio público (Locales) (NO ASEGURABLE)',
];

export const DEPENDENCIA_OPTIONS: Dependencia[] = [
  'SEP',
  'ISSSTE JUBILADOS',
  'ISSSTE',
  'ISSEMYM',
  'IMSS JUBILADOS',
  'IMSS',
  'GOBIERNO DEL ESTADO DE MEXICO',
  'GOB CDMX',
  'EMPRESA PRIVADA',
  'DIF MUNICIPAL TOLUCA',
  'UAQ',
  'GUARDIA NACIONAL Y SERVIDOR PUBLICO ARMADO',
  'OTRA DEPENDENCIA SERVIDOR PUBLICO',
];

export const METODO_PAGO_OPTIONS: MetodoPago[] = [
  'Descuento por Nomina',
  'Tarjeta de Credito',
  'Tarjeta de Debito',
  'Cuenta CLABE (OJO en esta modalidad no hay mas de 3 reintentos)',
];

export const CLAVE_DELEGACIONAL_OPTIONS: ClaveDelegacional[] = ['15', '16', '18', '23', '39'];

export const TIPO_CONTRATO_OPTIONS: TipoContrato[] = [
  '00',
  '01',
  '02',
  '04',
  '07',
  '08',
  '09',
  '10',
  '11',
];

export const UBICACION_SUBDELEGACION_OPTIONS: UbicacionSubdelegacion[] = ['Si', 'No'];

export type Estado =
  | 'Ciudad de México'
  | 'Estado de México'
  | 'Aguascalientes'
  | 'Baja California'
  | 'Baja California Sur'
  | 'Campeche'
  | 'Chiapas'
  | 'Chihuahua'
  | 'Coahuila'
  | 'Colima'
  | 'Durango'
  | 'Guanajuato'
  | 'Guerrero'
  | 'Hidalgo'
  | 'Jalisco'
  | 'Michoacán'
  | 'Morelos'
  | 'Nayarit'
  | 'Nuevo León'
  | 'Oaxaca'
  | 'Puebla'
  | 'Querétaro'
  | 'Quintana Roo'
  | 'San Luis Potosí'
  | 'Sinaloa'
  | 'Sonora'
  | 'Tabasco'
  | 'Tamaulipas'
  | 'Tlaxcala'
  | 'Veracruz'
  | 'Yucatán'
  | 'Zacatecas';

export type ProfesionAsegurado =
  | 'Empleado'
  | 'Maestro o Docente'
  | 'Administrativo'
  | 'Almacen'
  | 'Doctor'
  | 'Enfermera'
  | 'Personal de apoyo medico'
  | 'Chofer'
  | 'Cobrador'
  | 'Colocación y/o mantenimiento de anuncios'
  | 'Electricista'
  | 'Inmersiones submarina (hasta 40 metros)'
  | 'Instalación y/o mantenimiento de antenas'
  | 'Limpiador de cristales y/o chimenea'
  | 'Mensajero en motocicleta'
  | 'Mineros (Sin manejo de explosivos y hasta 2 días a la semana en mina)'
  | 'Químico Radiología'
  | 'Reparador/Instalador de elevadores'
  | 'Policías'
  | 'Aviación (empleado en talleres y pilotos)'
  | 'Barquero (embarcación en aguas tranquilas o poco profundas)'
  | 'Cargador'
  | 'Chef o Cocinero'
  | 'Mudanzas'
  | 'Venta o instalación de aire Acondicionado'
  | 'Azafata (NO ASEGURABLE)'
  | 'Bombero (NO ASEGURABLE)'
  | 'Calderero (Refinería de petróleo) (NO ASEGURABLE)'
  | 'Carcelero (NO ASEGURABLE)'
  | 'Chofer (Vehículos blindados. Traslado de valores) (NO ASEGURABLE)'
  | 'Diputado (NO ASEGURABLE)'
  | 'Gobernador (NO ASEGURABLE)'
  | 'Guardaespaldas (NO ASEGURABLE)'
  | 'Guardia forestal (NO ASEGURABLE)'
  | 'Inmersiones submarina (más de 40 metros) (NO ASEGURABLE)'
  | 'Magistrados /Juez (Federales) (NO ASEGURABLE)'
  | 'Manejo o contacto con explosivos (NO ASEGURABLE)'
  | 'Marina (navegación) (NO ASEGURABLE)'
  | 'Militar (NO ASEGURABLE)'
  | 'Mineros (Manejo de explosivos y/o mayor o igual a 3 días a la semana en mina) (NO ASEGURABLE)'
  | 'Ministerio público (Locales) (NO ASEGURABLE)';

export const ALCALDIA_OPTIONS: Alcaldia[] = [
  'Azcapotzalco',
  'Cuajimalpa',
  'Cuauhtemoc',
  'Gustavo A Madero',
  'Iztacalco',
  'Iztapalapa',
  'Miguel Hidalgo',
  'Tlahuac',
  'Venustiano Carranza',
];

export const ESTADO_OPTIONS: Estado[] = [
  'Ciudad de México',
  'Estado de México',
  'Aguascalientes',
  'Baja California',
  'Baja California Sur',
  'Campeche',
  'Chiapas',
  'Chihuahua',
  'Coahuila',
  'Colima',
  'Durango',
  'Guanajuato',
  'Guerrero',
  'Hidalgo',
  'Jalisco',
  'Michoacán',
  'Morelos',
  'Nayarit',
  'Nuevo León',
  'Oaxaca',
  'Puebla',
  'Querétaro',
  'Quintana Roo',
  'San Luis Potosí',
  'Sinaloa',
  'Sonora',
  'Tabasco',
  'Tamaulipas',
  'Tlaxcala',
  'Veracruz',
  'Yucatán',
  'Zacatecas',
];

export const PROFESION_ASEGURADO_OPTIONS: ProfesionAsegurado[] = [
  'Empleado',
  'Maestro o Docente',
  'Administrativo',
  'Almacen',
  'Doctor',
  'Enfermera',
  'Personal de apoyo medico',
  'Chofer',
  'Cobrador',
  'Colocación y/o mantenimiento de anuncios',
  'Electricista',
  'Inmersiones submarina (hasta 40 metros)',
  'Instalación y/o mantenimiento de antenas',
  'Limpiador de cristales y/o chimenea',
  'Mensajero en motocicleta',
  'Mineros (Sin manejo de explosivos y hasta 2 días a la semana en mina)',
  'Químico Radiología',
  'Reparador/Instalador de elevadores',
  'Policías',
  'Aviación (empleado en talleres y pilotos)',
  'Barquero (embarcación en aguas tranquilas o poco profundas)',
  'Cargador',
  'Chef o Cocinero',
  'Mudanzas',
  'Venta o instalación de aire Acondicionado',
  'Azafata (NO ASEGURABLE)',
  'Bombero (NO ASEGURABLE)',
  'Calderero (Refinería de petróleo) (NO ASEGURABLE)',
  'Carcelero (NO ASEGURABLE)',
  'Chofer (Vehículos blindados. Traslado de valores) (NO ASEGURABLE)',
  'Diputado (NO ASEGURABLE)',
  'Gobernador (NO ASEGURABLE)',
  'Guardaespaldas (NO ASEGURABLE)',
  'Guardia forestal (NO ASEGURABLE)',
  'Inmersiones submarina (más de 40 metros) (NO ASEGURABLE)',
  'Magistrados /Juez (Federales) (NO ASEGURABLE)',
  'Manejo o contacto con explosivos (NO ASEGURABLE)',
  'Marina (navegación) (NO ASEGURABLE)',
  'Militar (NO ASEGURABLE)',
  'Mineros (Manejo de explosivos y/o mayor o igual a 3 días a la semana en mina) (NO ASEGURABLE)',
  'Ministerio público (Locales) (NO ASEGURABLE)',
];
