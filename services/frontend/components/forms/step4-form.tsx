'use client';

import React from 'react';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Card } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { EditableField } from '@/components/ui/editable-field';
import { step4Schema, type Step4Schema } from '@/lib/validations/step4-schema';
import { useFormContext } from '@/contexts/form-context';
import type { Step4FormData } from '@/types/step4-form';

interface Step4FormProps {
  onNext: (data: Step4FormData) => void;
  onPrevious: (data: Step4FormData) => void;
}

export function Step4Form({ onNext, onPrevious }: Step4FormProps) {
  const { step4Data } = useFormContext();
  
  const {
    control,
    handleSubmit,
    formState: { errors },
    getValues,
  } = useForm<Step4Schema>({
    resolver: zodResolver(step4Schema),
    defaultValues: step4Data || {
      claveAgente: '',
      tieneCedulaVigente: undefined,
      rfc: '',
      correo: '',
    },
  });

  const onSubmit = (data: Step4Schema) => {
    onNext(data);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
      <Card className="p-6">
        <div className="flex items-center gap-3 mb-6">
          <svg
            className="h-6 w-6 text-blue-600"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"
            />
          </svg>
          <h2 className="text-xl font-semibold">Información del Agente</h2>
        </div>

        <div className="space-y-6">
          {/* Clave de Agente */}
          <div className="space-y-2">
            <Controller
              name="claveAgente"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="¿Cuál es tu clave de agente?"
                  value={field.value}
                  onChange={field.onChange}
                  placeholder="Ingresa tu clave de agente"
                  required
                  error={errors.claveAgente?.message}
                />
              )}
            />
          </div>

          {/* Cédula Vigente */}
          <div className="space-y-2">
            <label className="block text-sm font-medium text-gray-700">
              ¿Tienes cédula de agente vigente? <span className="text-red-500">*</span>
            </label>
            <Controller
              name="tieneCedulaVigente"
              control={control}
              render={({ field }) => (
                <div className="space-y-2">
                  <div className="flex items-center gap-6">
                    <label className="flex items-center gap-2 cursor-pointer">
                      <input
                        type="radio"
                        value="si"
                        checked={field.value === 'si'}
                        onChange={() => field.onChange('si')}
                        className="w-4 h-4 text-blue-600 border-gray-300 focus:ring-blue-500"
                      />
                      <span className="text-sm text-gray-700">Sí</span>
                    </label>
                    <label className="flex items-center gap-2 cursor-pointer">
                      <input
                        type="radio"
                        value="no"
                        checked={field.value === 'no'}
                        onChange={() => field.onChange('no')}
                        className="w-4 h-4 text-blue-600 border-gray-300 focus:ring-blue-500"
                      />
                      <span className="text-sm text-gray-700">No</span>
                    </label>
                  </div>
                  {errors.tieneCedulaVigente && (
                    <p className="text-sm text-red-600">{errors.tieneCedulaVigente.message}</p>
                  )}
                </div>
              )}
            />
          </div>

          {/* RFC */}
          <div className="space-y-2">
            <Controller
              name="rfc"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="¿Cuál es tu RFC?"
                  value={field.value}
                  onChange={(value) => field.onChange(value.toUpperCase())}
                  placeholder="Ej: ABCD123456XXX"
                  required
                  error={errors.rfc?.message}
                />
              )}
            />
          </div>

          {/* Correo */}
          <div className="space-y-2">
            <Controller
              name="correo"
              control={control}
              render={({ field }) => (
                <EditableField
                  label="¿Cuál es tu correo?"
                  value={field.value}
                  onChange={field.onChange}
                  placeholder="correo@ejemplo.com"
                  required
                  error={errors.correo?.message}
                />
              )}
            />
          </div>
        </div>
      </Card>

      {/* Navigation Buttons */}
      <div className="flex justify-between">
        <Button
          type="button"
          variant="outline"
          onClick={() => onPrevious(getValues())}
          className="min-w-[120px]"
        >
          Anterior
        </Button>
        <Button
          type="submit"
          className="min-w-[120px]"
        >
          Siguiente
        </Button>
      </div>
    </form>
  );
}
