'use client';

import * as React from 'react';
import { cn } from '@/lib/utils';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Separator } from '@/components/ui/separator';
import { EditableField } from '@/components/ui/editable-field';
import { AlertBanner } from '@/components/ui/alert-banner';
import type { ExtractedInfo, BannerInfo } from '@/types/file-upload';
import { GenderMap } from '@/types/gender';

interface ExtractedInfoPanelProps {
  data: ExtractedInfo;
  onDataChange: (data: ExtractedInfo) => void;
  isProcessing: boolean;
  completedFiles: number;
  totalRequiredFiles: number;
  showPaycheckInfo?: boolean;
  className?: string;
}

// Icons
const FileTextIcon = ({ className }: { className?: string }) => (
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
    <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z" />
    <polyline points="14 2 14 8 20 8" />
    <line x1="16" y1="13" x2="8" y2="13" />
    <line x1="16" y1="17" x2="8" y2="17" />
  </svg>
);

const ReceiptIcon = ({ className }: { className?: string }) => (
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
    <path d="M4 2v20l2-1 2 1 2-1 2 1 2-1 2 1 2-1 2 1V2l-2 1-2-1-2 1-2-1-2 1-2-1-2 1Z" />
    <path d="M16 8h-6a2 2 0 1 0 0 4h4a2 2 0 1 1 0 4H8" />
    <path d="M12 17.5v-11" />
  </svg>
);

const IdCardIcon = ({ className }: { className?: string }) => (
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
    <rect width="20" height="14" x="2" y="5" rx="2" />
    <line x1="2" x2="22" y1="10" y2="10" />
  </svg>
);

const ScanLineIcon = ({ className }: { className?: string }) => (
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
    <path d="M3 7V5a2 2 0 0 1 2-2h2" />
    <path d="M17 3h2a2 2 0 0 1 2 2v2" />
    <path d="M21 17v2a2 2 0 0 1-2 2h-2" />
    <path d="M7 21H5a2 2 0 0 1-2-2v-2" />
    <path d="M7 12h10" />
  </svg>
);

export function ExtractedInfoPanel({
  data,
  onDataChange,
  isProcessing,
  completedFiles,
  totalRequiredFiles,
  showPaycheckInfo = true,
  className,
}: ExtractedInfoPanelProps) {
  const allFilesProcessed = completedFiles >= totalRequiredFiles;
  const [banner, setBanner] = React.useState<BannerInfo | null>(null);

  // Helper to update nested paycheck data
  const updatePaycheck = (field: keyof ExtractedInfo['paycheck'], value: string) => {
    onDataChange({
      ...data,
      paycheck: { ...data.paycheck, [field]: value || null },
    });
  };

  // Helper to update nested idFront data
  const updateIdFront = (field: keyof ExtractedInfo['idFront'], value: string) => {
    onDataChange({
      ...data,
      idFront: { ...data.idFront, [field]: value || null },
    });
  };

  // Helper to update nested idBack data
  const updateIdBack = (field: keyof ExtractedInfo['idBack'], value: string) => {
    onDataChange({
      ...data,
      idBack: { ...data.idBack, [field]: value || '' },
    });
  };

  // Check which sections have data
  const paycheckProcessing = isProcessing && completedFiles < 1;
  const idFrontProcessing = isProcessing && completedFiles < 2;
  const idBackProcessing = isProcessing && completedFiles < 3;

  // Handle banner dismiss
  const handleBannerDismiss = () => {
    setBanner(null);
  };

  React.useEffect(() => {
    if (allFilesProcessed && !isProcessing) {
      var messages = [];
      if(data.idFront.domicilio.esta_completado === false) {
        messages.push('El domicilio extraído del INE está incompleto. Por favor, corrígelo antes de continuar.');
      }

      if (messages.length > 0) {
        setBanner({
          type: 'warning',
          message: messages,
        });
      } else {
        setBanner(null);
      }
    }
  }, [data, isProcessing, allFilesProcessed]);

  return (
    <div className={cn('space-y-4', className)}>
      {/* Alert Banner */}
      <AlertBanner banner={banner} onDismiss={handleBannerDismiss} />

      {/* Main Card */}
      <Card className="transition-all duration-300">
        <CardHeader className="pb-4">
          <div className="flex items-center justify-between">
            <CardTitle className="text-lg font-semibold flex items-center gap-2">
              <FileTextIcon className="h-5 w-5 text-primary" />
              Información Extraída
            </CardTitle>
            <div className="flex items-center gap-2">
              <span className="text-xs text-muted-foreground">
                {completedFiles}/{totalRequiredFiles} archivos procesados
              </span>
              {isProcessing && (
                <div className="flex items-center gap-1.5">
                  <div className="h-2 w-2 rounded-full bg-blue-500 animate-pulse" />
                  <span className="text-xs text-blue-600 dark:text-blue-400">
                    Procesando...
                  </span>
                </div>
              )}
              {allFilesProcessed && !isProcessing && (
                <div className="flex items-center gap-1.5">
                  <div className="h-2 w-2 rounded-full bg-green-500" />
                  <span className="text-xs text-green-600 dark:text-green-400">
                    Listo
                  </span>
                </div>
              )}
            </div>
          </div>
        </CardHeader>

        <Separator />

        <CardContent className="pt-6 space-y-8">
          {/* Paycheck Section */}
          {showPaycheckInfo && (
            <div className="space-y-4">
              <div className="flex items-center gap-2 text-sm font-medium text-foreground">
                <ReceiptIcon className="h-4 w-4 text-muted-foreground" />
                Información del Recibo de Nómina
              </div>
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 pl-6">
              <EditableField
                label="Matrícula"
                value={data.paycheck?.matricula}
                onChange={(v) => updatePaycheck('matricula', v)}
                isLoading={paycheckProcessing}
                disabled
              />
              <EditableField
                label="Tipo de Contratación"
                value={data.paycheck.tipo_de_contratacion?.toString() ?? ""}
                onChange={(v) => updatePaycheck('tipo_de_contratacion', v)}
                isLoading={paycheckProcessing}
                disabled
              />
              <EditableField
                label="Clave Est Org"
                value={data.paycheck.clave_est_org}
                onChange={(v) => updatePaycheck('clave_est_org', v)}
                isLoading={paycheckProcessing}
                disabled
              />
              <EditableField
                label="RFC"
                value={data.paycheck.RFC}
                onChange={(v) => updatePaycheck('RFC', v)}
                isLoading={paycheckProcessing}
                disabled
              />
            </div>
          </div>
          )}

          {showPaycheckInfo && <Separator />}

          {/* ID Front Section */}
          <div className="space-y-4">
            <div className="flex items-center gap-2 text-sm font-medium text-foreground">
              <IdCardIcon className="h-4 w-4 text-muted-foreground" />
              Información INE Frente
            </div>
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 pl-6">
              <EditableField
                label="Nombre"
                value={data.idFront.nombre}
                onChange={(v) => updateIdFront('nombre', v)}
                isLoading={idFrontProcessing}
                disabled
              />
              <EditableField
                label="Apellido Paterno"
                value={data.idFront.apellido_paterno}
                onChange={(v) => updateIdFront('apellido_paterno', v)}
                isLoading={idFrontProcessing}
                disabled
              />
              <EditableField
                label="Apellido Materno"
                value={data.idFront.apellido_materno}
                onChange={(v) => updateIdFront('apellido_materno', v)}
                isLoading={idFrontProcessing}
                disabled
              />
              <EditableField
                label="Domicilio"
                value={!data.idFront.domicilio ? "" : data.idFront.domicilio.esta_completado ? `C ${data.idFront.domicilio.calle} ${data.idFront.domicilio.numero_exterior} COL ${data.idFront.domicilio.colonia} ${data.idFront.domicilio.codigo_postal}, ${data.idFront.domicilio.municipio}, ${data.idFront.domicilio.estado}` : 'INCOMPLETO'}
                onChange={(v) => {}}
                isLoading={idFrontProcessing}
                className="sm:col-span-3"
                disabled
              />
              <EditableField
                label="CURP"
                value={data.idFront.curp}
                onChange={(v) => updateIdFront('curp', v)}
                isLoading={idFrontProcessing}
                disabled
              />
              <EditableField
                label="Fecha de Nacimiento"
                value={!!data.idFront.fecha_nacimiento ? new Date(data.idFront.fecha_nacimiento)?.toISOString().split('T')[0] : null}
                onChange={(v) => updateIdFront('fecha_nacimiento', v)}
                isLoading={idFrontProcessing}
                disabled
              />
              <EditableField
                label="Sexo"
                value={data.idFront.sexo ? GenderMap[data.idFront.sexo.toString() as keyof typeof GenderMap] : ''}
                onChange={(v) => updateIdFront('sexo', v)}
                isLoading={idFrontProcessing}
                disabled
              />
            </div>
          </div>

          <Separator />

          {/* ID Back Section */}
          <div className="space-y-4">
            <div className="flex items-center gap-2 text-sm font-medium text-foreground">
              <ScanLineIcon className="h-4 w-4 text-muted-foreground" />
              Información INE Reverso
            </div>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 pl-6">
              <EditableField
                label="Validation Code"
                value={data.idBack.idmex}
                onChange={(v) => updateIdBack('idmex', v)}
                isLoading={idBackProcessing}
                disabled
              />
            </div>
          </div>

          {/* Empty state message */}
          {completedFiles === 0 && !isProcessing && (
            <div className="p-4 rounded-lg bg-muted/50 text-center">
              <p className="text-sm text-muted-foreground">
                Upload and process the required files to extract information automatically.
              </p>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}