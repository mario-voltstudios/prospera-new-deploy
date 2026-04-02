'use client';

import { Button } from '@/components/ui/button';
import { StepIndicator } from '@/components/forms/step-indicator';
import { Step1Form } from '@/components/forms/step1-form';
import { Step2Form } from '@/components/forms/step2-form';
import { Step3Form } from '@/components/forms/step3-form';
import { Step4Form } from '@/components/forms/step4-form';
import { FormProvider, useFormContext } from '@/contexts/form-context';
import type { Step2FormData } from '@/lib/validations/step2-schema';
import type { Step3FormData } from '@/lib/validations/step3-schema';
import type { Step4FormData } from '@/types/step4-form';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { getSession } from '@/api/agent.request';
import { Step1FormData } from '@/lib/validations/upload-schema';
import { ExtractedInfo } from '@/types/file-upload';
import { GenderMap } from '@/types/gender';

// Form steps configuration
const FORM_STEPS = [
  { label: 'Documentos', description: 'Subir archivos' },
  { label: 'Información de Contratación', description: 'Detalles personales' },
  { label: 'Firma de Contrato', description: 'Datos de firma' },
  { label: 'Información del Agente', description: 'Datos del agente' },
  { label: 'Enviar', description: 'Completar' },
];

function CreateFormContent() {
  const { currentStep, goToNextStep, goToPreviousStep, setStep2Data, setStep3Data, setStep4Data } = useFormContext();

  useQuery({queryKey: ['sessionKey'], queryFn: getSession})


   const handleStep1Next = (data: ExtractedInfo) => {
    setStep2Data({
      nombresContratante: `${data.idFront.nombre}`,
      segundoApellidoContratante: data.idFront.apellido_materno,
      primerApellidoContratante: data.idFront.apellido_paterno,
      generoContratante: GenderMap[data.idFront.sexo.toString() as keyof typeof GenderMap] as any,
      anioNacimientoContratante: data.idFront.fecha_nacimiento ? new Date(data.idFront.fecha_nacimiento).getFullYear().toString() : null!,
      diaNacimientoContratante: data.idFront.fecha_nacimiento ? new Date(data.idFront.fecha_nacimiento).getDate().toString() : null!,
      mesNacimientoContratante: data.idFront.fecha_nacimiento ? (new Date(data.idFront.fecha_nacimiento).getMonth() + 1).toString() : null!,
      rfcContratante: data.paycheck.RFC,
      calleContratante: data.idFront.domicilio.calle,
      numeroExteriorContratante: data.idFront.domicilio.numero_exterior,
      numeroInteriorContratante: data.idFront.domicilio.numero_interior,
      coloniaContratante: data.idFront.domicilio.colonia,
      codigoPostalContratante: data.idFront.domicilio.codigo_postal,
      municipioContratante: data.idFront.domicilio.municipio,
      estadoContratante: data.idFront.domicilio.estado,
      paisContratante: 'MEXICO',
      numeroIdentificacion: data.idBack.idmex,
      tipoIdentificacion: 'INE',
      nacionalidad: 'MEXICANA',
    })
    goToNextStep()
  }
  
  // Handle step 2 next
  const handleStep2Next = (data: Step2FormData) => {
    setStep2Data(data);
    goToNextStep();
    console.log('Step 2 data:', data);
  };

  const handleGoBackStep1 = (data: Step2FormData) => {
    setStep2Data(data);
    goToPreviousStep();
  }

  // Handle step 3 next
  const handleStep3Next = (data: Step3FormData) => {
    setStep3Data(data);
    goToNextStep();
    console.log('Step 3 data:', data);
  };

  const handleGoBackStep2 = (data: Step3FormData) => {
    setStep3Data(data);
    goToPreviousStep();
  }

  // Handle step 4 next
  const handleStep4Next = (data: Step4FormData) => {
    setStep4Data(data);
    goToNextStep();
    console.log('Step 4 data:', data);
  };

  const handleGobackStep3 = (data: Step4FormData) => {
    setStep4Data(data);
    goToPreviousStep();
  }

  return (
    <div className="min-h-screen bg-linear-to-br from-background via-background to-muted/20">
      <div className="container mx-auto px-4 py-8 max-w-5xl">
        {/* Header */}
        <div className="text-center mb-8">
          <h1 className="text-3xl font-bold text-foreground mb-2">
            {currentStep === 1 && 'Carga de Documentos'}
            {currentStep === 2 && 'Información de Contratación'}
            {currentStep === 3 && 'Firma de Contrato'}
            {currentStep === 4 && 'Información del Agente'}
            {currentStep === 5 && 'Revisión y Envío'}
          </h1>
          <p className="text-muted-foreground">
            {currentStep === 1 && "Suba sus documentos para comenzar. Extraeremos automáticamente la información necesaria."}
            {currentStep === 2 && 'Por favor proporcione su información personal y profesional.'}
            {currentStep === 3 && 'Ingrese los datos de firma del contrato.'}
            {currentStep === 4 && 'Proporcione su información como agente de ventas.'}
            {currentStep === 5 && 'Revise toda la información antes de enviar.'}
          </p>
        </div>

        {/* Step Indicator */}
        <div className="mb-10">
          <StepIndicator
            currentStep={currentStep}
            totalSteps={FORM_STEPS.length}
            steps={FORM_STEPS}
          />
        </div>

        {/* Step 1: File Upload */}
        {currentStep === 1 && <Step1Form onNext={handleStep1Next} />}

        {/* Step 2: Hire Information */}
        {currentStep === 2 && (
          <Step2Form
            onNext={handleStep2Next}
            onPrevious={handleGoBackStep1}
          />
        )}

        {/* Step 3: Contract Signing */}
        {currentStep === 3 && (
          <Step3Form
            onNext={handleStep3Next}
            onPrevious={handleGoBackStep2}
          />
        )}

        {/* Step 4: Agent Information */}
        {currentStep === 4 && (
          <Step4Form
            onNext={handleStep4Next}
            onPrevious={goToPreviousStep}
          />
        )}

        {/* Step 5: Review & Submit */}
        {currentStep === 5 && (
          <div className="text-center py-16">
            <h2 className="text-2xl font-semibold mb-4">Paso 5 Próximamente</h2>
            <p className="text-muted-foreground mb-8">
              La funcionalidad de revisión y envío se implementará aquí.
            </p>
            <Button onClick={goToPreviousStep} variant="outline">
              Volver
            </Button>
          </div>
        )}
      </div>
    </div>
  );
}

export default function CreateFormPage() {
  return (
    <FormProvider>
      <CreateFormContent />
    </FormProvider>
  );
}
