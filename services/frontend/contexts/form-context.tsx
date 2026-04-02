'use client';

import * as React from 'react';
import type { ExtractedInfo, BannerInfo, FileUploadState, FileType } from '@/types/file-upload';
import type { Step2FormData } from '@/lib/validations/step2-schema';
import type { Step3FormData } from '@/lib/validations/step3-schema';
import type { Step4FormData } from '@/types/step4-form';
import { HiringType } from '@/types/hiringType';
import { Gender } from '@/types/gender';

interface FormContextValue {
  // Current step
  currentStep: number;
  setCurrentStep: (step: number) => void;
  
  // Step 1 data
  extractedInfo: ExtractedInfo;
  setExtractedInfo: React.Dispatch<React.SetStateAction<ExtractedInfo>>;
  fileUploadStates: Record<FileType, FileUploadState>;
  setFileUploadStates: React.Dispatch<React.SetStateAction<Record<FileType, FileUploadState>>>;
  hasTalones: boolean;
  setHasTalones: React.Dispatch<React.SetStateAction<boolean>>;
  
  // Step 2 data
  step2Data: Partial<Step2FormData>;
  setStep2Data: React.Dispatch<React.SetStateAction<Partial<Step2FormData>>>;
  
  // Step 3 data
  step3Data: Partial<Step3FormData>;
  setStep3Data: React.Dispatch<React.SetStateAction<Partial<Step3FormData>>>;
  
  // Step 4 data
  step4Data: Partial<Step4FormData>;
  setStep4Data: React.Dispatch<React.SetStateAction<Partial<Step4FormData>>>;
  
  // Navigation methods
  goToNextStep: () => void;
  goToPreviousStep: () => void;
  goToStep: (step: number) => void;
  
  // Reset methods
  resetForm: () => void;
}

const FormContext = React.createContext<FormContextValue | undefined>(undefined);

// Initial empty state for extracted info
const INITIAL_EXTRACTED_INFO: ExtractedInfo = {
  paycheck: {
    nombre: "",
    tipo_de_contratacion: null,
    matricula: "",
    clave_est_org: "",
    curp: "",
    RFC: "",
    type: null,
    percepciones: [],
    deducciones: [],
    observaciones: [],
    contains_gnp_policy: false,
  },
  idFront: {
    apellido_materno: "",
    apellido_paterno: "",
    nombre: "",
    domicilio: null!,
    curp: "",
    fecha_nacimiento: null!,
    sexo: Gender.M,
  },
  idBack: {
    idmex: "",
  },
};

const INITIAL_FILE_UPLOAD_STATES: Record<FileType, FileUploadState> = {
  paycheck: { file: null, preview: null, status: 'idle', progress: 0 },
  idFront: { file: null, preview: null, status: 'idle', progress: 0 },
  idBack: { file: null, preview: null, status: 'idle', progress: 0 },
  letter: { file: null, preview: null, status: 'idle', progress: 0 },
  proofOfAddress: { file: null, preview: null, status: 'idle', progress: 0 },
  picture: { file: null, preview: null, status: 'idle', progress: 0 },
};

interface FormProviderProps {
  children: React.ReactNode;
}

export function FormProvider({ children }: FormProviderProps) {
  const [currentStep, setCurrentStep] = React.useState(1);
  const [extractedInfo, setExtractedInfo] = React.useState<ExtractedInfo>(INITIAL_EXTRACTED_INFO);
  const [fileUploadStates, setFileUploadStates] = React.useState<Record<FileType, FileUploadState>>(INITIAL_FILE_UPLOAD_STATES);
  const [hasTalones, setHasTalones] = React.useState(false);

  const [step2Data, setStep2Data] = React.useState<Partial<Step2FormData>>({});
  const [step3Data, setStep3Data] = React.useState<Partial<Step3FormData>>({});
  const [step4Data, setStep4Data] = React.useState<Partial<Step4FormData>>({});

  const goToNextStep = React.useCallback(() => {
    setCurrentStep((prev) => prev + 1);
  }, []);

  const goToPreviousStep = React.useCallback(() => {
    setCurrentStep((prev) => Math.max(1, prev - 1));
  }, []);

  const goToStep = React.useCallback((step: number) => {
    setCurrentStep(step);
  }, []);

  const resetForm = React.useCallback(() => {
    setCurrentStep(1);
    setExtractedInfo(INITIAL_EXTRACTED_INFO);
    setFileUploadStates(INITIAL_FILE_UPLOAD_STATES);
    setHasTalones(false);
    setStep2Data({});
    setStep3Data({});
    setStep4Data({});
  }, []);

  const value = React.useMemo<FormContextValue>(
    () => ({
      currentStep,
      setCurrentStep,
      extractedInfo,
      setExtractedInfo,
      fileUploadStates,
      setFileUploadStates,
      hasTalones,
      setHasTalones,
      step2Data,
      setStep2Data,
      step3Data,
      setStep3Data,
      step4Data,
      setStep4Data,
      goToNextStep,
      goToPreviousStep,
      goToStep,
      resetForm,
    }),
    [
      currentStep,
      extractedInfo,
      fileUploadStates,
      hasTalones,
      step2Data,
      step3Data,
      step4Data,
      goToNextStep,
      goToPreviousStep,
      goToStep,
      resetForm,
    ]
  );

  return <FormContext.Provider value={value}>{children}</FormContext.Provider>;
}

export function useFormContext() {
  const context = React.useContext(FormContext);
  if (context === undefined) {
    throw new Error('useFormContext must be used within a FormProvider');
  }
  return context;
}
