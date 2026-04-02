// Contract sign information types

export const ESTADOS_REPUBLICA = [
  'Ciudad de México',
  'Estado de México',
  'Morelos',
  'Querétaro',
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
  'Nayarit',
  'Nuevo León',
  'Oaxaca',
  'Puebla',
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
] as const;

export const ALCALDIAS_CDMX = [
  'Álvaro Obregón',
  'Azcapotzalco',
  'Benito Juárez',
  'Coyoacán',
  'Cuajimalpa de Morelos',
  'Cuauhtémoc',
  'Gustavo A. Madero',
  'Iztacalco',
  'Iztapalapa',
  'La Magdalena Contreras',
  'Miguel Hidalgo',
  'Milpa Alta',
  'Tláhuac',
  'Tlalpan',
  'Venustiano Carranza',
  'Xochimilco',
] as const;

export const MUNICIPIOS_EDOMEX = [
  'Acolman',
  'Almoloya de Juárez',
  'Atizapán de Zaragoza',
  'Atlacomulco',
  'Chalco',
  'Chicoloapan',
  'Chimalhuacán',
  'Coacalco de Berriozábal',
  'Cuautitlán',
  'Cuautitlán Izcalli',
  'Ecatepec de Morelos',
  'Huehuetoca',
  'Huixquilucan',
  'Ixtapaluca',
  'Ixtlahuaca',
  'La Paz',
  'Lerma',
  'Metepec',
  'Naucalpan de Juárez',
  'Nezahualcóyotl',
  'Nicolás Romero',
  'San Felipe del Progreso',
  'Tecámac',
  'Temoaya',
  'Tenancingo',
  'Tepotzotlán',
  'Texcoco',
  'Tlalnepantla de Baz',
  'Toluca',
  'Tultepec',
  'Tultitlán',
  'Valle de Chalco Solidaridad',
  'Villa Victoria',
  'Zinacantepec',
  'Zumpango',
  'Acambay de Ruíz Castañeda',
  'Aculco',
  'Almoloya de Alquisiras',
  'Almoloya del Río',
  'Amanalco',
  'Amatepec',
  'Amecameca',
  'Apaxco',
  'Atenco',
  'Atizapán',
  'Atlautla',
  'Axapusco',
  'Ayapango',
  'Calimaya',
  'Capulhuac',
  'Chapa de Mota',
  'Chapultepec',
  'Chiautla',
  'Chiconcuac',
  'Coatepec Harinas',
  'Cocotitlán',
  'Coyotepec',
  'Donato Guerra',
  'Ecatzingo',
  'El Oro',
  'Hueypoxtla',
  'Isidro Fabela',
  'Ixtapan de la Sal',
  'Ixtapan del Oro',
  'Jaltenco',
  'Jilotepec',
  'Jilotzingo',
  'Jiquipilco',
  'Jocotitlán',
  'Joquicingo',
  'Juchitepec',
  'Luvianos',
  'Malinalco',
  'Melchor Ocampo',
  'Mexicaltzingo',
  'Morelos',
  'Nextlalpan',
  'Nopaltepec',
  'Ocoyoacac',
  'Ocuilan',
  'Otumba',
  'Otzoloapan',
  'Otzolotepec',
  'Ozumba',
  'Papalotla',
  'Polotitlán',
  'Rayón',
  'San Antonio la Isla',
  'San José del Rincón',
  'San Martín de las Pirámides',
  'San Mateo Atenco',
  'San Simón de Guerrero',
  'Santo Tomás',
  'Soyaniquilpan de Juárez',
  'Sultepec',
  'Tejupilco',
  'Temamatla',
  'Temascalapa',
  'Temascalcingo',
  'Temascaltepec',
  'Tenango del Aire',
  'Tenango del Valle',
  'Teoloyucan',
  'Teotihuacán',
  'Tepetlaoxtoc',
  'Tepetlixpa',
  'Tequixquiac',
  'Texcaltitlán',
  'Texcalyacac',
  'Tezoyuca',
  'Tianguistenco',
  'Timilpan',
  'Tlalmanalco',
  'Tlatlaya',
  'Tonanitla',
  'Tonatico',
  'Valle de Bravo',
  'Villa de Allende',
  'Villa del Carbón',
  'Villa Guerrero',
  'Xalatlaco',
  'Xonacatlán',
  'Zacazonapan',
  'Zacualpan',
  'Zumpahuacán',
] as const;

export const MUNICIPIOS_POR_ESTADO: Record<string, readonly string[]> = {
  'Ciudad de México': ALCALDIAS_CDMX,
  'Estado de México': MUNICIPIOS_EDOMEX,
  'Morelos': [],
  'Querétaro': [],
  'Aguascalientes': [],
  'Baja California': [],
  'Baja California Sur': [],
  'Campeche': [],
  'Chiapas': [],
  'Chihuahua': [],
  'Coahuila': [],
  'Colima': [],
  'Durango': [],
  'Guanajuato': [],
  'Guerrero': [],
  'Hidalgo': [],
  'Jalisco': [],
  'Michoacán': [],
  'Nayarit': [],
  'Nuevo León': [],
  'Oaxaca': [],
  'Puebla': [],
  'Quintana Roo': [],
  'San Luis Potosí': [],
  'Sinaloa': [],
  'Sonora': [],
  'Tabasco': [],
  'Tamaulipas': [],
  'Tlaxcala': [],
  'Veracruz': [],
  'Yucatán': [],
  'Zacatecas': [],
};

export type EstadoRepublica = typeof ESTADOS_REPUBLICA[number];

export type PeriodoPago = 'Quincenal' | 'Mensual' | 'Anual';

export type TipoCotizacion = 'Prima, quiere pagar cierta cantidad' | 'Suma Asegurada, quiere cierta cantidad de proteccion';

export const PERIODO_PAGO_OPTIONS: PeriodoPago[] = ['Quincenal', 'Mensual', 'Anual'];

export const TIPO_COTIZACION_OPTIONS: TipoCotizacion[] = [
  'Prima, quiere pagar cierta cantidad',
  'Suma Asegurada, quiere cierta cantidad de proteccion',
];

export interface Beneficiario {
  nombresBeneficiario: string;
  primerApellidoBeneficiario: string;
  segundoApellidoBeneficiario: string;
  parentescoBeneficiario: string;
  diaNacimientoBeneficiario: string;
  mesNacimientoBeneficiario: string;
  anoNacimientoBeneficiario: string;
  porcentajeSumaAsegurada: string;
}

export interface Step3FormData {
  diaFirma: string;
  mesFirma: string;
  anoFirma: string;
  estadoVenta: EstadoRepublica;
  municipioVenta: string;

  // Policy Configuration
  periodoPago: PeriodoPago;
  tipoCotizacion: TipoCotizacion;
  sumaAseguradaCotizada: string;
  primaSeguroRiesgoAnual: string;
  primaAhorroExcedenteAnual: string;
  aseguradoTieneOtrasPolizas: 'Si' | 'No';

  // Payment Details
  numeroTarjeta: string;
  fechaVencimientoTarjeta: string;
  clabe: string;
  banco: string;
  fechaProximoCobro: string;

  // Beneficiaries
  beneficiarios: Beneficiario[];
}
