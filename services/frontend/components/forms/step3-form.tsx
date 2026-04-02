'use client';

import * as React from 'react';
import { useForm, Controller, useFieldArray } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { EditableField } from '@/components/ui/editable-field';
import { SelectField } from '@/components/ui/select-field';
import { useFormContext } from '@/contexts/form-context';
import { step3Schema, type Step3FormData } from '@/lib/validations/step3-schema';
import {
  ESTADOS_REPUBLICA,
  MUNICIPIOS_POR_ESTADO,
  PERIODO_PAGO_OPTIONS,
  TIPO_COTIZACION_OPTIONS,
} from '@/types/step3-form';

interface Step3FormProps {
  onNext: (data: Step3FormData) => void;
  onPrevious: (data: Step3FormData) => void;
}

// Calendar Icon
const CalendarIcon = ({ className }: { className?: string }) => (
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
    <rect x="3" y="4" width="18" height="18" rx="2" ry="2" />
    <line x1="16" y1="2" x2="16" y2="6" />
    <line x1="8" y1="2" x2="8" y2="6" />
    <line x1="3" y1="10" x2="21" y2="10" />
  </svg>
);

// Location Icon
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

// Generate day options (1-31)
const DAY_OPTIONS = Array.from({ length: 31 }, (_, i) => String(i + 1));

// Generate month options (1-12)
const MONTH_OPTIONS = Array.from({ length: 12 }, (_, i) => String(i + 1));

// Generate year options (current year - 10 to current year + 1)
const currentYear = new Date().getFullYear();
const YEAR_OPTIONS = Array.from({ length: 12 }, (_, i) => String(currentYear - 10 + i));

export function Step3Form({ onNext, onPrevious }: Step3FormProps) {
  const { step3Data } = useFormContext();
  
  const {
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
    watch,
    getValues,
    setValue,
  } = useForm<Step3FormData>({
    resolver: zodResolver(step3Schema),
    defaultValues: step3Data || {
      diaFirma: '',
      mesFirma: '',
      anoFirma: '',
      estadoVenta: '' as any,
      municipioVenta: '',
      periodoPago: '' as any,
      tipoCotizacion: '' as any,
      sumaAseguradaCotizada: '',
      primaSeguroRiesgoAnual: '',
      primaAhorroExcedenteAnual: '',
      aseguradoTieneOtrasPolizas: 'No',
      numeroTarjeta: '',
      fechaVencimientoTarjeta: '',
      clabe: '',
      banco: '',
      fechaProximoCobro: '',
      beneficiarios: [
        {
          nombresBeneficiario: '',
          primerApellidoBeneficiario: '',
          segundoApellidoBeneficiario: '',
          parentescoBeneficiario: '',
          diaNacimientoBeneficiario: '',
          mesNacimientoBeneficiario: '',
          anoNacimientoBeneficiario: '',
          porcentajeSumaAsegurada: '',
        },
      ],
    },
  });

  const selectedEstado = watch('estadoVenta');
  const { fields: beneficiaryFields, append, remove } = useFieldArray({
    control,
    name: 'beneficiarios',
  });
  
  // Get available municipalities based on selected state
  const availableMunicipios = React.useMemo(() => {
    if (!selectedEstado) return [];
    return MUNICIPIOS_POR_ESTADO[selectedEstado] || [];
  }, [selectedEstado]);

  // Reset municipality when state changes
  React.useEffect(() => {
    if (selectedEstado) {
      setValue('municipioVenta', '');
    }
  }, [selectedEstado, setValue]);

  const onSubmit = (data: Step3FormData) => {
    onNext(data);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
      {/* Date Section */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <CalendarIcon className="h-5 w-5 text-primary" />
            Fecha de Firma del Contrato
          </CardTitle>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <Controller
              name="diaFirma"
              control={control}
              render={({ field }) => (
                <SelectField
                  label="Día"
                  value={field.value}
                  onChange={field.onChange}
                  options={DAY_OPTIONS}
                  placeholder="Selecciona el día"
                  required
                  error={errors.diaFirma?.message}
                />
              )}
            />
            <Controller
              name="mesFirma"
              control={control}
              render={({ field }) => (
                <SelectField
                  label="Mes"
                  value={field.value}
                  onChange={field.onChange}
                  options={MONTH_OPTIONS}
                  placeholder="Selecciona el mes"
                  required
                  error={errors.mesFirma?.message}
                />
              )}
            />
            <Controller
              name="anoFirma"
              control={control}
              render={({ field }) => (
                <SelectField
                  label="Año"
                  value={field.value}
                  onChange={field.onChange}
                  options={YEAR_OPTIONS}
                  placeholder="Selecciona el año"
                  required
                  error={errors.anoFirma?.message}
                />
              )}
            />
          </div>
        </CardContent>
      </Card>

      {/* Location Section */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <MapPinIcon className="h-5 w-5 text-primary" />
            Ubicación de la Venta
          </CardTitle>
        </CardHeader>
        <CardContent className="space-y-4">
          <Controller
            name="estadoVenta"
            control={control}
            render={({ field }) => (
              <SelectField
                label="Estado de la República"
                value={field.value}
                onChange={field.onChange}
                options={[...ESTADOS_REPUBLICA]}
                placeholder="Selecciona el estado"
                required
                error={errors.estadoVenta?.message}
              />
            )}
          />
          
          <Controller
            name="municipioVenta"
            control={control}
            render={({ field }) => (
              <SelectField
                label="Alcaldía o Municipio"
                value={field.value}
                onChange={field.onChange}
                options={[...availableMunicipios]}
                placeholder={
                  selectedEstado
                    ? availableMunicipios.length > 0
                      ? 'Selecciona el municipio o alcaldía'
                      : 'No hay municipios disponibles para este estado'
                    : 'Primero selecciona un estado'
                }
                required
                error={errors.municipioVenta?.message}
              />
            )}
          />
        </CardContent>
      </Card>

      {/* Policy Configuration Section */}
      <Card>
        <CardHeader>
          <CardTitle className="text-xl font-bold flex items-center gap-2">
            <CalendarIcon className="w-6 h-6" />
            Configuración de la Póliza
          </CardTitle>
        </CardHeader>
        <CardContent className="space-y-6">
          <Controller
            name="periodoPago"
            control={control}
            render={({ field }) => (
              <SelectField
                label="Periodo de Pago"
                value={field.value}
                onChange={field.onChange}
                options={PERIODO_PAGO_OPTIONS}
                placeholder="Selecciona el periodo de pago"
                required
                error={errors.periodoPago?.message}
              />
            )}
          />

          <Controller
            name="tipoCotizacion"
            control={control}
            render={({ field }) => (
              <SelectField
                label="¿Le cotizaste por prima o por suma asegurada?"
                value={field.value}
                onChange={field.onChange}
                options={TIPO_COTIZACION_OPTIONS}
                placeholder="Selecciona el tipo de cotización"
                required
                error={errors.tipoCotizacion?.message}
              />
            )}
          />

          <Controller
            name="sumaAseguradaCotizada"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Suma Asegurada que cotizaste"
                value={field.value}
                onChange={field.onChange}
                placeholder="Ingresa la suma asegurada"
                required
                error={errors.sumaAseguradaCotizada?.message}
              />
            )}
          />

          <Controller
            name="primaSeguroRiesgoAnual"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Prima del seguro/riesgo que cotizaste, de manera ANUAL"
                value={field.value}
                onChange={field.onChange}
                placeholder="Ingresa la prima del seguro/riesgo anual"
                required
                error={errors.primaSeguroRiesgoAnual?.message}
              />
            )}
          />

          <Controller
            name="primaAhorroExcedenteAnual"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Prima de ahorro o excedente que cotizaste, de manera ANUAL"
                value={field.value}
                onChange={field.onChange}
                placeholder="Ingresa la prima de ahorro o excedente anual"
                required
                error={errors.primaAhorroExcedenteAnual?.message}
              />
            )}
          />

          <div className="space-y-2">
            <label className="text-sm font-medium">
              El ASEGURADO tiene ya otras pólizas
              <span className="text-red-500 ml-1">*</span>
            </label>
            <Controller
              name="aseguradoTieneOtrasPolizas"
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
            {errors.aseguradoTieneOtrasPolizas?.message && (
              <p className="text-sm text-red-500">{errors.aseguradoTieneOtrasPolizas.message}</p>
            )}
          </div>
        </CardContent>
      </Card>

      {/* Credit Card Section */}
      <Card>
        <CardHeader>
          <CardTitle className="text-xl font-bold flex items-center gap-2">
            <CalendarIcon className="w-6 h-6" />
            Tarjeta de Crédito
          </CardTitle>
        </CardHeader>
        <CardContent className="space-y-6">
          <Controller
            name="numeroTarjeta"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Número de Tarjeta"
                value={field.value}
                onChange={field.onChange}
                placeholder="Ingresa el número de tarjeta"
                required
                error={errors.numeroTarjeta?.message}
              />
            )}
          />

          <Controller
            name="fechaVencimientoTarjeta"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Fecha de Vencimiento"
                value={field.value}
                onChange={field.onChange}
                placeholder="MM/AA"
                required
                error={errors.fechaVencimientoTarjeta?.message}
              />
            )}
          />
        </CardContent>
      </Card>

      {/* Bank Account Section */}
      <Card>
        <CardHeader>
          <CardTitle className="text-xl font-bold flex items-center gap-2">
            <MapPinIcon className="w-6 h-6" />
            Cuenta Bancaria
          </CardTitle>
        </CardHeader>
        <CardContent className="space-y-6">
          <Controller
            name="clabe"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Número Interbancaria (CLABE)"
                value={field.value}
                onChange={field.onChange}
                placeholder="18 dígitos"
                required
                error={errors.clabe?.message}
              />
            )}
          />

          <Controller
            name="banco"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Banco"
                value={field.value}
                onChange={field.onChange}
                required
                error={errors.banco?.message}
              />
            )}
          />

          <Controller
            name="fechaProximoCobro"
            control={control}
            render={({ field }) => (
              <EditableField
                label="Fecha de Próximo Cobro"
                value={field.value}
                onChange={field.onChange}
                placeholder="DD/MM/AAAA"
                required
                error={errors.fechaProximoCobro?.message}
              />
            )}
          />
        </CardContent>
      </Card>

      {/* Beneficiaries Section */}
      <Card>
        <CardHeader>
          <CardTitle className="text-xl font-bold flex items-center gap-2">
            <MapPinIcon className="w-6 h-6" />
            Beneficiarios
          </CardTitle>
        </CardHeader>
        <CardContent className="space-y-6">
          {beneficiaryFields.map((field, index) => (
            <div key={field.id} className="space-y-4 border border-muted/50 rounded-lg p-4">
              <div className="flex justify-between items-center">
                <p className="text-sm font-semibold">Beneficiario {index + 1}</p>
                {beneficiaryFields.length > 1 && (
                  <Button variant="outline" size="sm" onClick={() => remove(index)}>
                    Eliminar
                  </Button>
                )}
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                <Controller
                  name={`beneficiarios.${index}.nombresBeneficiario` as const}
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Nombres Beneficiario"
                      value={field.value}
                      onChange={field.onChange}
                      required
                      error={errors.beneficiarios?.[index]?.nombresBeneficiario?.message}
                    />
                  )}
                />
                <Controller
                  name={`beneficiarios.${index}.primerApellidoBeneficiario` as const}
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Primer Apellido Beneficiario"
                      value={field.value}
                      onChange={field.onChange}
                      required
                      error={errors.beneficiarios?.[index]?.primerApellidoBeneficiario?.message}
                    />
                  )}
                />
                <Controller
                  name={`beneficiarios.${index}.segundoApellidoBeneficiario` as const}
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Segundo Apellido Beneficiario"
                      value={field.value}
                      onChange={field.onChange}
                      required
                      error={errors.beneficiarios?.[index]?.segundoApellidoBeneficiario?.message}
                    />
                  )}
                />
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                <Controller
                  name={`beneficiarios.${index}.parentescoBeneficiario` as const}
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Parentesco Beneficiario"
                      value={field.value}
                      onChange={field.onChange}
                      required
                      error={errors.beneficiarios?.[index]?.parentescoBeneficiario?.message}
                    />
                  )}
                />
                <Controller
                  name={`beneficiarios.${index}.diaNacimientoBeneficiario` as const}
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Día de Nacimiento"
                      value={field.value}
                      onChange={field.onChange}
                      required
                      error={errors.beneficiarios?.[index]?.diaNacimientoBeneficiario?.message}
                    />
                  )}
                />
                <Controller
                  name={`beneficiarios.${index}.mesNacimientoBeneficiario` as const}
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Mes de Nacimiento"
                      value={field.value}
                      onChange={field.onChange}
                      required
                      error={errors.beneficiarios?.[index]?.mesNacimientoBeneficiario?.message}
                    />
                  )}
                />
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                <Controller
                  name={`beneficiarios.${index}.anoNacimientoBeneficiario` as const}
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Año de Nacimiento"
                      value={field.value}
                      onChange={field.onChange}
                      required
                      error={errors.beneficiarios?.[index]?.anoNacimientoBeneficiario?.message}
                    />
                  )}
                />
                <Controller
                  name={`beneficiarios.${index}.porcentajeSumaAsegurada` as const}
                  control={control}
                  render={({ field }) => (
                    <EditableField
                      label="Porcentaje de Suma Asegurada para Beneficiario"
                      value={field.value}
                      onChange={field.onChange}
                      required
                      error={errors.beneficiarios?.[index]?.porcentajeSumaAsegurada?.message}
                    />
                  )}
                />
              </div>
            </div>
          ))}

          {errors.beneficiarios?.message && (
            <p className="text-sm text-red-500">{errors.beneficiarios?.message}</p>
          )}

          <Button size="sm" type="button" onClick={() =>
            append({
              nombresBeneficiario: '',
              primerApellidoBeneficiario: '',
              segundoApellidoBeneficiario: '',
              parentescoBeneficiario: '',
              diaNacimientoBeneficiario: '',
              mesNacimientoBeneficiario: '',
              anoNacimientoBeneficiario: '',
              porcentajeSumaAsegurada: '',
            })
          }>
            Agregar Beneficiario
          </Button>
        </CardContent>
      </Card>

      {/* Navigation Buttons */}
      <div className="flex justify-between gap-4">
        <Button
          type="button"
          variant="outline"
          onClick={() => onPrevious(getValues())}
          size="lg"
          className="min-w-[200px]"
        >
          Anterior
        </Button>
        <Button
          type="submit"
          size="lg"
          className="min-w-[200px]"
          disabled={isSubmitting}
        >
          {isSubmitting ? 'Guardando...' : 'Siguiente'}
        </Button>
      </div>
    </form>
  );
}
