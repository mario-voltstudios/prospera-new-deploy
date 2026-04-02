'use client';

import * as React from 'react';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Separator } from '@/components/ui/separator';
import { EditableField } from '@/components/ui/editable-field';
import { SelectField } from '@/components/ui/select-field';
import { FileUpload } from '@/components/ui/file-upload';
import { useFormContext } from '@/contexts/form-context';
import { step2Schema, type Step2FormData } from '@/lib/validations/step2-schema';
import {
  GENDER_OPTIONS,
  REGIMEN_FISCAL_OPTIONS,
  TIPO_IDENTIFICACION_OPTIONS,
  PROFESION_OPTIONS,
  DEPENDENCIA_OPTIONS,
  METODO_PAGO_OPTIONS,
  CLAVE_DELEGACIONAL_OPTIONS,
  TIPO_CONTRATO_OPTIONS,
  UBICACION_SUBDELEGACION_OPTIONS,
  ALCALDIA_OPTIONS,
  ESTADO_OPTIONS,
  PROFESION_ASEGURADO_OPTIONS,
} from '@/types/step2-form';

interface Step2FormProps {
  onNext: (data: Step2FormData) => void;
  onPrevious: (data: Step2FormData) => void;
}

// Icons
const UserIcon = ({ className }: { className?: string }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <path d="M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2" />
    <circle cx="12" cy="7" r="4" />
  </svg>
);

const MapPinIcon = ({ className }: { className?: string }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <path d="M20 10c0 6-8 12-8 12s-8-6-8-12a8 8 0 0 1 16 0Z" />
    <circle cx="12" cy="10" r="3" />
  </svg>
);

const PhoneIcon = ({ className }: { className?: string }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z" />
  </svg>
);

const BriefcaseIcon = ({ className }: { className?: string }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <rect width="20" height="14" x="2" y="7" rx="2" ry="2" />
    <path d="M16 21V5a2 2 0 0 0-2-2h-4a2 2 0 0 0-2 2v16" />
  </svg>
);

export function Step2Form({ onNext, onPrevious }: Step2FormProps) {
  const { step2Data } = useFormContext();
  
  const {
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
    getValues,
    watch,
  } = useForm<Step2FormData>({
    resolver: zodResolver(step2Schema),
    defaultValues: step2Data || {
      nombresContratante: '',
      primerApellidoContratante: '',
      segundoApellidoContratante: '',
      diaNacimientoContratante: '',
      mesNacimientoContratante: '',
      anioNacimientoContratante: '',
      generoContratante: '' as any,
      rfcContratante: '',
      entidadFederativa: '',
      nacionalidad: '',
      regimenFiscal: '605 SUELDOS Y SALARIOS E INGRESOS ASIMILADOS A SALARIOS' as any,
      tipoIdentificacion: '' as any,
      organismoEmiteIdentificacion: '',
      numeroIdentificacion: '',
      calleContratante: '',
      numeroExteriorContratante: '',
      numeroInteriorContratante: '',
      codigoPostalContratante: '',
      coloniaContratante: '',
      estadoContratante: '',
      municipioContratante: '',
      paisContratante: '',
      correoElectronicoContratante: '',
      telefonoMovilContratante: '',
      profesionContratante: '' as any,
      dependenciaEmpresa: '' as any,
      metodoPago: '' as any,
      aseguradoEsContratante: 'Si',
      direccionAseguradoEsContratante: 'Si',
      padeceEnfermedad: 'No',
      padecidoCovid19: 'No',
      requirioAsistenciaRespiratoria: 'No',
      aseguradoFuma: 'No',
    },
  });

  const aseguradoEsContratante = watch('aseguradoEsContratante');
  const direccionAseguradoEsContratante = watch('direccionAseguradoEsContratante');

  const onSubmit = (data: Step2FormData) => {
    onNext(data);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-8">
      {/* Section 1: Personal Information */}
      <Card>
        <CardHeader>
          <CardTitle className="text-lg font-semibold flex items-center gap-2">
            <UserIcon className="h-5 w-5 text-primary" />
            Personal Information
          </CardTitle>
        </CardHeader>
        <Separator />
        <CardContent className="pt-6 space-y-4">
          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <Controller
              name="nombresContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Nombres"
                  value={field.value}
                  onChange={field.onChange}
                  required
                  error={errors.nombresContratante?.message}
                />
              )}
            />
            <Controller
              name="primerApellidoContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Primer Apellido"
                  value={field.value}
                  onChange={field.onChange}
                  required
                  error={errors.primerApellidoContratante?.message}
                />
              )}
            />
            <Controller
              name="segundoApellidoContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Segundo Apellido"
                  value={field.value}
                  onChange={field.onChange}
                  required
                  error={errors.segundoApellidoContratante?.message}
                />
              )}
            />
          </div>

          <div className="grid grid-cols-1 sm:grid-cols-4 gap-4">
            <Controller
              name="diaNacimientoContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Día Nacimiento"
                  value={field.value}
                  onChange={field.onChange}
                  placeholder="01-31"
                  required
                  error={errors.diaNacimientoContratante?.message}
                />
              )}
            />
            <Controller
              name="mesNacimientoContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Mes Nacimiento"
                  value={field.value}
                  onChange={field.onChange}
                  placeholder="01-12"
                  required
                  error={errors.mesNacimientoContratante?.message}
                />
              )}
            />
            <Controller
              name="anioNacimientoContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Año Nacimiento"
                  value={field.value}
                  onChange={field.onChange}
                  placeholder="1990"
                  required
                  error={errors.anioNacimientoContratante?.message}
                />
              )}
            />
            <Controller
              name="generoContratante"
              control={control}
              render={({ field }) => (
                <SelectField
                  label="Género"
                  value={field.value}
                  onChange={field.onChange}
                  options={GENDER_OPTIONS}
                  required
                  error={errors.generoContratante?.message}
                />
              )}
            />
          </div>

          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <Controller
              name="rfcContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="RFC"
                  value={field.value}
                  onChange={field.onChange}
                  placeholder="XAXX010101000"
                  required
                  error={errors.rfcContratante?.message}
                />
              )}
            />
            <Controller
              name="nacionalidad"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Nacionalidad"
                  value={field.value}
                  onChange={field.onChange}
                  required
                  error={errors.nacionalidad?.message}
                />
              )}
            />
          </div>

          <Controller
            name="entidadFederativa"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Entidad Federativa y País de Nacimiento"
                value={field.value ?? ''}
                onChange={field.onChange}
                error={errors.entidadFederativa?.message}
              />
            )}
          />

          <Controller
            name="regimenFiscal"
            control={control}
            render={({ field }) => (
              <SelectField
                label="Régimen Fiscal"
                value={field.value}
                onChange={field.onChange}
                options={REGIMEN_FISCAL_OPTIONS}
                required
                error={errors.regimenFiscal?.message}
              />
            )}
          />

          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <Controller
              name="tipoIdentificacion"
              control={control}
              render={({ field }) => (
                <SelectField
                  label="Tipo de Identificación"
                  value={field.value}
                  onChange={field.onChange}
                  options={TIPO_IDENTIFICACION_OPTIONS}
                  required
                  error={errors.tipoIdentificacion?.message}
                />
              )}
            />
            <Controller
              name="organismoEmiteIdentificacion"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Organismo Emisor"
                  value={field.value}
                  onChange={field.onChange}
                  required
                  error={errors.organismoEmiteIdentificacion?.message}
                />
              )}
            />
            <Controller
              name="numeroIdentificacion"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Número de Identificación"
                  value={field.value}
                  onChange={field.onChange}
                  required
                  error={errors.numeroIdentificacion?.message}
                />
              )}
            />
          </div>
        </CardContent>
      </Card>

      {/* Section 2: Address */}
      <Card>
        <CardHeader>
          <CardTitle className="text-lg font-semibold flex items-center gap-2">
            <MapPinIcon className="h-5 w-5 text-primary" />
            Address
          </CardTitle>
        </CardHeader>
        <Separator />
        <CardContent className="pt-6 space-y-4">
          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <Controller
              name="calleContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Calle"
                  value={field.value}
                  onChange={field.onChange}
                  required
                  error={errors.calleContratante?.message}
                  className="sm:col-span-2"
                />
              )}
            />
            <Controller
              name="numeroExteriorContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Número Exterior"
                  value={field.value}
                  onChange={field.onChange}
                  required
                  error={errors.numeroExteriorContratante?.message}
                />
              )}
            />
          </div>

          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <Controller
              name="numeroInteriorContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Número Interior"
                  value={field.value ?? ''}
                  onChange={field.onChange}
                  error={errors.numeroInteriorContratante?.message}
                />
              )}
            />
            <Controller
              name="codigoPostalContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Código Postal"
                  value={field.value}
                  onChange={field.onChange}
                  placeholder="00000"
                  required
                  error={errors.codigoPostalContratante?.message}
                />
              )}
            />
            <Controller
              name="coloniaContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Colonia"
                  value={field.value}
                  onChange={field.onChange}
                  required
                  error={errors.coloniaContratante?.message}
                />
              )}
            />
          </div>

          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <Controller
              name="estadoContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Estado"
                  value={field.value}
                  onChange={field.onChange}
                  required
                  error={errors.estadoContratante?.message}
                />
              )}
            />
            <Controller
              name="municipioContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Municipio o Alcaldía"
                  value={field.value}
                  onChange={field.onChange}
                  required
                  error={errors.municipioContratante?.message}
                />
              )}
            />
            <Controller
              name="paisContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="País"
                  value={field.value}
                  onChange={field.onChange}
                  required
                  error={errors.paisContratante?.message}
                />
              )}
            />
          </div>
        </CardContent>
      </Card>

      {/* Section 3: Contact Information */}
      <Card>
        <CardHeader>
          <CardTitle className="text-lg font-semibold flex items-center gap-2">
            <PhoneIcon className="h-5 w-5 text-primary" />
            Contact Information
          </CardTitle>
        </CardHeader>
        <Separator />
        <CardContent className="pt-6 space-y-4">
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <Controller
              name="correoElectronicoContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Correo Electrónico"
                  value={field.value}
                  onChange={field.onChange}
                  type="email"
                  placeholder="email@example.com"
                  required
                  error={errors.correoElectronicoContratante?.message}
                />
              )}
            />
            <Controller
              name="telefonoMovilContratante"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="Teléfono Móvil"
                  value={field.value}
                  onChange={field.onChange}
                  type="tel"
                  placeholder="5512345678"
                  required
                  error={errors.telefonoMovilContratante?.message}
                />
              )}
            />
          </div>
        </CardContent>
      </Card>

      {/* Section 4: Professional Information */}
      <Card>
        <CardHeader>
          <CardTitle className="text-lg font-semibold flex items-center gap-2">
            <BriefcaseIcon className="h-5 w-5 text-primary" />
            Professional Information
          </CardTitle>
        </CardHeader>
        <Separator />
        <CardContent className="pt-6 space-y-4">
          <Controller
            name="profesionContratante"
            control={control}
            render={({ field }) => (
              <SelectField
                label="Profesión u Ocupación"
                value={field.value}
                onChange={field.onChange}
                options={PROFESION_OPTIONS}
                required
                error={errors.profesionContratante?.message}
              />
            )}
          />

          <Controller
            name="dependenciaEmpresa"
            control={control}
            render={({ field }) => (
              <SelectField
                label="Dependencia o Empresa"
                value={field.value}
                onChange={field.onChange}
                options={DEPENDENCIA_OPTIONS}
                required
                error={errors.dependenciaEmpresa?.message}
              />
            )}
          />

          <Controller
            name="metodoPago"
            control={control}
            render={({ field }) => (
              <SelectField
                label="Método de Pago"
                value={field.value}
                onChange={field.onChange}
                options={METODO_PAGO_OPTIONS}
                required
                error={errors.metodoPago?.message}
              />
            )}
          />
        </CardContent>
      </Card>

      {/* Section 5: IMSS Information */}
      <Card>
        <CardHeader>
          <CardTitle className="text-lg font-semibold flex items-center gap-2">
            <BriefcaseIcon className="h-5 w-5 text-primary" />
            Información IMSS
          </CardTitle>
        </CardHeader>
        <Separator />
        <CardContent className="pt-6 space-y-4">
          <Controller
            name="claveDelegacionalIMSS"
            control={control}
            render={({ field }) => (
              <SelectField
                label="Clave Delegacional IMSS"
                value={field.value}
                onChange={field.onChange}
                options={CLAVE_DELEGACIONAL_OPTIONS}
                required
                error={errors.claveDelegacionalIMSS?.message}
              />
            )}
          />

          <Controller
            name="matriculaIMSS"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Matrícula (Llave de descuento)"
                value={field.value}
                onChange={field.onChange}
                required
                error={errors.matriculaIMSS?.message}
              />
            )}
          />

          <Controller
            name="tipoContratoIMSS"
            control={control}
            render={({ field }) => (
              <SelectField
                label="Tipo de Contrato"
                value={field.value}
                onChange={field.onChange}
                options={TIPO_CONTRATO_OPTIONS}
                required
                error={errors.tipoContratoIMSS?.message}
              />
            )}
          />

          <Controller
            name="nombreUbicacion"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Nombre de la Ubicación"
                value={field.value}
                onChange={field.onChange}
                required
                error={errors.nombreUbicacion?.message}
              />
            )}
          />

          <Controller
            name="ubicacionSubdelegacion"
            control={control}
            render={({ field }) => (
              <SelectField
                label="¿La ubicación es de una subdelegación diferente a las del Estado de México, Querétaro, Morelos o Oaxaca?"
                value={field.value}
                onChange={field.onChange}
                options={UBICACION_SUBDELEGACION_OPTIONS}
                required
                error={errors.ubicacionSubdelegacion?.message}
              />
            )}
          />
        </CardContent>
      </Card>

      {/* Section 6: GOB CDMX Information */}
      <Card>
        <CardHeader>
          <CardTitle className="text-lg font-semibold flex items-center gap-2">
            <BriefcaseIcon className="h-5 w-5 text-primary" />
            Información GOB CDMX
          </CardTitle>
        </CardHeader>
        <Separator />
        <CardContent className="pt-6 space-y-4">
          <Controller
            name="numeroEmpleadoCDMX"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Número del Empleado"
                value={field.value}
                onChange={field.onChange}
                required
                error={errors.numeroEmpleadoCDMX?.message}
              />
            )}
          />

          <Controller
            name="alcaldiaCDMX"
            control={control}
            render={({ field }) => (
              <SelectField
                label="Alcaldía"
                value={field.value}
                onChange={field.onChange}
                options={ALCALDIA_OPTIONS}
                required
                error={errors.alcaldiaCDMX?.message}
              />
            )}
          />

          <Controller
            name="edificioUbicacionCDMX"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Edificio o Ubicación"
                value={field.value}
                onChange={field.onChange}
                required
                error={errors.edificioUbicacionCDMX?.message}
              />
            )}
          />
        </CardContent>
      </Card>

      {/* Section 7: SEP Information */}
      <Card>
        <CardHeader>
          <CardTitle className="text-lg font-semibold flex items-center gap-2">
            <BriefcaseIcon className="h-5 w-5 text-primary" />
            Información SEP
          </CardTitle>
        </CardHeader>
        <Separator />
        <CardContent className="pt-6 space-y-4">
          <Controller
            name="centroTrabajoSEP"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Ingresa el Centro de trabajo completo"
                value={field.value}
                onChange={field.onChange}
                required
                error={errors.centroTrabajoSEP?.message}
              />
            )}
          />

          <Controller
            name="primerosDosDigitosCCT"
            control={control}
            render={({ field }) => (
              <EditableField
                label="2 Primeros dígitos del CCT"
                value={field.value}
                onChange={field.onChange}
                maxLength={2}
                required
                error={errors.primerosDosDigitosCCT?.message}
              />
            )}
          />

          <Controller
            name="cartaAutorizacionSEP"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Carga Carta Autorización SEP"
                value={field.value ?? ''}
                onChange={field.onChange}
                error={errors.cartaAutorizacionSEP?.message}
              />
            )}
          />
        </CardContent>
      </Card>

      {/* Section 8: ISSEMYM Information */}
      <Card>
        <CardHeader>
          <CardTitle className="text-lg font-semibold flex items-center gap-2">
            <BriefcaseIcon className="h-5 w-5 text-primary" />
            Información ISSEMYM
          </CardTitle>
        </CardHeader>
        <Separator />
        <CardContent className="pt-6 space-y-4">
          <Controller
            name="claveISSEMYM"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Clave ISSEMYM"
                value={field.value}
                onChange={field.onChange}
                required
                error={errors.claveISSEMYM?.message}
              />
            )}
          />
        </CardContent>
      </Card>

      {/* Section 9: Insured Person Information */}
      <Card>
        <CardHeader>
          <CardTitle className="text-lg font-semibold flex items-center gap-2">
            <UserIcon className="h-5 w-5 text-primary" />
            Información del Asegurado
          </CardTitle>
        </CardHeader>
        <Separator />
        <CardContent className="pt-6 space-y-4">
          <div className="space-y-2">
            <label className="text-sm font-medium">
              ¿El asegurado es el mismo que el contratante?
              <span className="text-red-500 ml-1">*</span>
            </label>
            <Controller
              name="aseguradoEsContratante"
              control={control}
              render={({ field }) => (
                <div className="flex gap-4">
                  <label className="flex items-center gap-2 cursor-pointer">
                    <input
                      type="radio"
                      value="Si"
                      checked={field.value === 'Si'}
                      onChange={() => field.onChange('Si')}
                      className="w-4 h-4 text-primary"
                    />
                    <span className="text-sm">Sí</span>
                  </label>
                  <label className="flex items-center gap-2 cursor-pointer">
                    <input
                      type="radio"
                      value="No"
                      checked={field.value === 'No'}
                      onChange={() => field.onChange('No')}
                      className="w-4 h-4 text-primary"
                    />
                    <span className="text-sm">No</span>
                  </label>
                </div>
              )}
            />
            {errors.aseguradoEsContratante?.message && (
              <p className="text-sm text-red-500">{errors.aseguradoEsContratante.message}</p>
            )}
          </div>

          {aseguradoEsContratante === 'No' && (
            <>
              <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                <Controller
                  name="nombresAsegurado"
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Nombres del Asegurado"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      required
                      error={errors.nombresAsegurado?.message}
                    />
                  )}
                />
                <Controller
                  name="primerApellidoAsegurado"
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Primer Apellido del Asegurado"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      required
                      error={errors.primerApellidoAsegurado?.message}
                    />
                  )}
                />
                <Controller
                  name="segundoApellidoAsegurado"
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Segundo Apellido del Asegurado"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      required
                      error={errors.segundoApellidoAsegurado?.message}
                    />
                  )}
                />
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-4 gap-4">
                <Controller
                  name="diaNacimientoAsegurado"
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Día Nacimiento"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      placeholder="01-31"
                      required
                      error={errors.diaNacimientoAsegurado?.message}
                    />
                  )}
                />
                <Controller
                  name="mesNacimientoAsegurado"
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Mes Nacimiento"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      placeholder="01-12"
                      required
                      error={errors.mesNacimientoAsegurado?.message}
                    />
                  )}
                />
                <Controller
                  name="anioNacimientoAsegurado"
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Año Nacimiento"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      placeholder="1990"
                      required
                      error={errors.anioNacimientoAsegurado?.message}
                    />
                  )}
                />
                <Controller
                  name="generoAsegurado"
                  control={control}
                  render={({ field }) => (
                    <SelectField
                      label="Género"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      options={GENDER_OPTIONS}
                      required
                      error={errors.generoAsegurado?.message}
                    />
                  )}
                />
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <Controller
                  name="rfcAsegurado"
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="RFC Asegurado"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      placeholder="XAXX010101000"
                      required
                      error={errors.rfcAsegurado?.message}
                    />
                  )}
                />
                <Controller
                  name="estadoNacimientoAsegurado"
                  control={control}
                  render={({ field }) => (
                    <SelectField
                      label="Estado de Nacimiento del Asegurado"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      options={ESTADO_OPTIONS}
                      required
                      error={errors.estadoNacimientoAsegurado?.message}
                    />
                  )}
                />
              </div>

              <Controller
                name="nacionalidadAsegurado"
                control={control}
                render={({ field }) => (
                  <EditableField
                    label="Nacionalidad del Asegurado"
                    value={field.value ?? ''}
                    onChange={field.onChange}
                    required
                    error={errors.nacionalidadAsegurado?.message}
                  />
                )}
              />

              <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                <Controller
                  name="tipoIdentificacionAsegurado"
                  control={control}
                  render={({ field }) => (
                    <SelectField
                      label="Tipo de Identificación Asegurado"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      options={TIPO_IDENTIFICACION_OPTIONS}
                      required
                      error={errors.tipoIdentificacionAsegurado?.message}
                    />
                  )}
                />
                <Controller
                  name="organismoEmiteIdentificacionAsegurado"
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Entidad que emite la identificación"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      required
                      error={errors.organismoEmiteIdentificacionAsegurado?.message}
                    />
                  )}
                />
                <Controller
                  name="numeroIdentificacionAsegurado"
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Número de Identificación"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      required
                      error={errors.numeroIdentificacionAsegurado?.message}
                    />
                  )}
                />
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <Controller
                  name="correoElectronicoAsegurado"
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Correo electrónico del asegurado"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      type="email"
                      placeholder="email@example.com"
                      error={errors.correoElectronicoAsegurado?.message}
                    />
                  )}
                />
                <Controller
                  name="telefonoMovilAsegurado"
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Teléfono móvil del asegurado"
                      value={field.value ?? ''}
                      onChange={field.onChange}
                      type="tel"
                      placeholder="5512345678"
                      error={errors.telefonoMovilAsegurado?.message}
                    />
                  )}
                />
              </div>

              <Controller
                name="profesionAsegurado"
                control={control}
                render={({ field }) => (
                  <SelectField
                    label="Profesión u ocupación del asegurado"
                    value={field.value ?? ''}
                    onChange={field.onChange}
                    options={PROFESION_ASEGURADO_OPTIONS}
                    required
                    error={errors.profesionAsegurado?.message}
                  />
                )}
              />

              <div className="space-y-2 pt-4">
                <label className="text-sm font-medium">
                  ¿La dirección del asegurado es la misma que el contratante?
                </label>
                <Controller
                  name="direccionAseguradoEsContratante"
                  control={control}
                  render={({ field }) => (
                    <div className="flex gap-4">
                      <label className="flex items-center gap-2 cursor-pointer">
                        <input
                          type="radio"
                          value="Si"
                          checked={field.value === 'Si'}
                          onChange={() => field.onChange('Si')}
                          className="w-4 h-4 text-primary"
                        />
                        <span className="text-sm">Sí</span>
                      </label>
                      <label className="flex items-center gap-2 cursor-pointer">
                        <input
                          type="radio"
                          value="No"
                          checked={field.value === 'No'}
                          onChange={() => field.onChange('No')}
                          className="w-4 h-4 text-primary"
                        />
                        <span className="text-sm">No</span>
                      </label>
                    </div>
                  )}
                />
                {errors.direccionAseguradoEsContratante?.message && (
                  <p className="text-sm text-red-500">{errors.direccionAseguradoEsContratante.message}</p>
                )}
              </div>

              {direccionAseguradoEsContratante === 'No' && (
                <>
                  <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                    <Controller
                      name="calleAsegurado"
                      control={control}
                      render={({ field }) => (
                        <EditableField
                          label="Calle del Asegurado"
                          value={field.value ?? ''}
                          onChange={field.onChange}
                          required
                          error={errors.calleAsegurado?.message}
                          className="sm:col-span-2"
                        />
                      )}
                    />
                    <Controller
                      name="numeroExteriorAsegurado"
                      control={control}
                      render={({ field }) => (
                        <EditableField
                          label="Número Exterior Asegurado"
                          value={field.value ?? ''}
                          onChange={field.onChange}
                          required
                          error={errors.numeroExteriorAsegurado?.message}
                        />
                      )}
                    />
                  </div>

                  <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                    <Controller
                      name="numeroInteriorAsegurado"
                      control={control}
                      render={({ field }) => (
                        <EditableField
                          label="Número Interior Asegurado"
                          value={field.value ?? ''}
                          onChange={field.onChange}
                          required
                          error={errors.numeroInteriorAsegurado?.message}
                        />
                      )}
                    />
                    <Controller
                      name="codigoPostalAsegurado"
                      control={control}
                      render={({ field }) => (
                        <EditableField
                          label="Código Postal Asegurado"
                          value={field.value ?? ''}
                          onChange={field.onChange}
                          placeholder="00000"
                          required
                          error={errors.codigoPostalAsegurado?.message}
                        />
                      )}
                    />
                    <Controller
                      name="coloniaAsegurado"
                      control={control}
                      render={({ field }) => (
                        <EditableField
                          label="Colonia Asegurado"
                          value={field.value ?? ''}
                          onChange={field.onChange}
                          required
                          error={errors.coloniaAsegurado?.message}
                        />
                      )}
                    />
                  </div>

                  <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                    <Controller
                      name="municipioAsegurado"
                      control={control}
                      render={({ field }) => (
                        <EditableField
                          label="Municipio o Alcaldía Asegurado"
                          value={field.value ?? ''}
                          onChange={field.onChange}
                          required
                          error={errors.municipioAsegurado?.message}
                        />
                      )}
                    />
                    <Controller
                      name="estadoAsegurado"
                      control={control}
                      render={({ field }) => (
                        <EditableField
                          label="Estado Asegurado"
                          value={field.value ?? ''}
                          onChange={field.onChange}
                          required
                          error={errors.estadoAsegurado?.message}
                        />
                      )}
                    />
                    <Controller
                      name="paisAsegurado"
                      control={control}
                      render={({ field }) => (
                        <EditableField
                          label="País Asegurado"
                          value={field.value ?? ''}
                          onChange={field.onChange}
                          required
                          error={errors.paisAsegurado?.message}
                        />
                      )}
                    />
                  </div>
                </>
              )}
            </>
          )}
        </CardContent>
      </Card>

      {/* Section 10: Health Declaration */}
      <Card>
        <CardHeader>
          <CardTitle className="text-lg font-semibold flex items-center gap-2">
            <UserIcon className="h-5 w-5 text-primary" />
            Declaración de Salud
          </CardTitle>
        </CardHeader>
        <Separator />
        <CardContent className="pt-6 space-y-4">
          <div className="space-y-2">
            <label className="text-sm font-medium">
              ¿El solicitante padece o ha padecido alguna enfermedad hepática, mental, pulmonar, renal, neurológica, cardiovascular,
              hipertensión arterial, diabetes, epilepsia, esclerosis múltiple, fiebre reumática, SIDA, cáncer, tumores, leucemia, lupus,
              alcoholismo, drogadicción o alguna otra enfermedad degenerativa no mencionada anteriormente?
              <span className="text-red-500 ml-1">*</span>
            </label>
            <Controller
              name="padeceEnfermedad"
              control={control}
              render={({ field }) => (
                <div className="flex gap-4">
                  <label className="flex items-center gap-2 cursor-pointer">
                    <input
                      type="radio"
                      value="Si"
                      checked={field.value === 'Si'}
                      onChange={() => field.onChange('Si')}
                      className="w-4 h-4 text-primary"
                    />
                    <span className="text-sm">Sí</span>
                  </label>
                  <label className="flex items-center gap-2 cursor-pointer">
                    <input
                      type="radio"
                      value="No"
                      checked={field.value === 'No'}
                      onChange={() => field.onChange('No')}
                      className="w-4 h-4 text-primary"
                    />
                    <span className="text-sm">No</span>
                  </label>
                </div>
              )}
            />
            {errors.padeceEnfermedad?.message && (
              <p className="text-sm text-red-500">{errors.padeceEnfermedad.message}</p>
            )}
          </div>

          <div className="space-y-2">
            <label className="text-sm font-medium">
              ¿El asegurado ha padecido COVID 19?
              <span className="text-red-500 ml-1">*</span>
            </label>
            <Controller
              name="padecidoCovid19"
              control={control}
              render={({ field }) => (
                <div className="flex gap-4">
                  <label className="flex items-center gap-2 cursor-pointer">
                    <input
                      type="radio"
                      value="Si"
                      checked={field.value === 'Si'}
                      onChange={() => field.onChange('Si')}
                      className="w-4 h-4 text-primary"
                    />
                    <span className="text-sm">Sí</span>
                  </label>
                  <label className="flex items-center gap-2 cursor-pointer">
                    <input
                      type="radio"
                      value="No"
                      checked={field.value === 'No'}
                      onChange={() => field.onChange('No')}
                      className="w-4 h-4 text-primary"
                    />
                    <span className="text-sm">No</span>
                  </label>
                </div>
              )}
            />
            {errors.padecidoCovid19?.message && (
              <p className="text-sm text-red-500">{errors.padecidoCovid19.message}</p>
            )}
          </div>

          <Controller
            name="diasUltimoResultadoPositivo"
            control={control}
            render={({ field }) => (
              <EditableField
                label="¿Hace cuantos días fue su último resultado positivo?"
                value={field.value ?? ''}
                onChange={field.onChange}
                required
                error={errors.diasUltimoResultadoPositivo?.message}
              />
            )}
          />

          <div className="space-y-2">
            <label className="text-sm font-medium">
              ¿Requirió asistencia respiratoria (Tanque de oxígeno, ventilador, etc)?
              <span className="text-red-500 ml-1">*</span>
            </label>
            <Controller
              name="requirioAsistenciaRespiratoria"
              control={control}
              render={({ field }) => (
                <div className="flex gap-4">
                  <label className="flex items-center gap-2 cursor-pointer">
                    <input
                      type="radio"
                      value="Si"
                      checked={field.value === 'Si'}
                      onChange={() => field.onChange('Si')}
                      className="w-4 h-4 text-primary"
                    />
                    <span className="text-sm">Sí</span>
                  </label>
                  <label className="flex items-center gap-2 cursor-pointer">
                    <input
                      type="radio"
                      value="No"
                      checked={field.value === 'No'}
                      onChange={() => field.onChange('No')}
                      className="w-4 h-4 text-primary"
                    />
                    <span className="text-sm">No</span>
                  </label>
                </div>
              )}
            />
            {errors.requirioAsistenciaRespiratoria?.message && (
              <p className="text-sm text-red-500">{errors.requirioAsistenciaRespiratoria.message}</p>
            )}
          </div>

          <div className="space-y-2">
            <label className="text-sm font-medium">
              ¿El asegurado fuma?
              <span className="text-red-500 ml-1">*</span>
            </label>
            <Controller
              name="aseguradoFuma"
              control={control}
              render={({ field }) => (
                <div className="flex gap-4">
                  <label className="flex items-center gap-2 cursor-pointer">
                    <input
                      type="radio"
                      value="Si"
                      checked={field.value === 'Si'}
                      onChange={() => field.onChange('Si')}
                      className="w-4 h-4 text-primary"
                    />
                    <span className="text-sm">Sí</span>
                  </label>
                  <label className="flex items-center gap-2 cursor-pointer">
                    <input
                      type="radio"
                      value="No"
                      checked={field.value === 'No'}
                      onChange={() => field.onChange('No')}
                      className="w-4 h-4 text-primary"
                    />
                    <span className="text-sm">No</span>
                  </label>
                </div>
              )}
            />
            {errors.aseguradoFuma?.message && (
              <p className="text-sm text-red-500">{errors.aseguradoFuma.message}</p>
            )}
          </div>
        </CardContent>
      </Card>

      {/* Navigation Buttons */}
      <div className="flex justify-between gap-4">
        <Button type="button" variant="outline" onClick={() => onPrevious(getValues())}>
          Previous
        </Button>
        <Button type="submit" disabled={isSubmitting} className="min-w-30">
          {isSubmitting ? 'Saving...' : 'Next'}
        </Button>
      </div>
    </form>
  );
}
