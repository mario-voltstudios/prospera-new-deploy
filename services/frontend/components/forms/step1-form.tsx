'use client';

import * as React from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Button } from '@/components/ui/button';
import { FileUpload } from '@/components/ui/file-upload';
import { ExtractedInfoPanel } from '@/components/forms/extracted-info-panel';
import { useFileUpload } from '@/hooks/use-file-upload';
import { useFormContext } from '@/contexts/form-context';
import { step1Schema, type Step1FormData } from '@/lib/validations/upload-schema';
import {
  ExtractedInfo,
  FILE_UPLOAD_CONFIGS,
  type FileType,
} from '@/types/file-upload';
import { useState, useEffect } from 'react';
import { PaycheckResult } from '@/types/paycheck-result';
import { IdResult, IdResultBack } from '@/types/id-result';
import { processIdBack, processIdFront, processLetter, processPaycheck } from '@/api/agent.request';
import { file } from 'zod';
import { useQueryClient } from '@tanstack/react-query';

type Step1FormProps = {
  onNext: (data: ExtractedInfo) => void;
};
export function Step1Form({ onNext }: Step1FormProps) {
  const {
    extractedInfo,
    setExtractedInfo,
    fileUploadStates,
    setFileUploadStates,
    hasTalones,
    setHasTalones,
  } = useFormContext()

  const queryClient = useQueryClient();
  var { sessionToken } = queryClient.getQueryData<{sessionToken: string}>(['sessionKey']) || { sessionToken: null };

  // React Hook Form setup
  const form = useForm<Step1FormData>({
    resolver: zodResolver(step1Schema),
    mode: 'onChange',
  });

  // File upload hooks for each file type
  const paycheckUpload = useFileUpload<PaycheckResult>({
    config: FILE_UPLOAD_CONFIGS.paycheck,
    uploadFunction: (file, onUploadProgress) => processPaycheck(sessionToken!, file, onUploadProgress),
    initialState: fileUploadStates.paycheck,
    onProcessingComplete: (result) => {
      // Mock: Update extracted info when paycheck is processed
      setExtractedInfo((prev) => ({
        ...prev,
        paycheck: { ...result },
      }));
    },
  });

  const idFrontUpload = useFileUpload<IdResult>({
    config: FILE_UPLOAD_CONFIGS.idFront,
    uploadFunction: (file, onUploadProgress) => processIdFront(sessionToken!, file, onUploadProgress),
    initialState: fileUploadStates.idFront,
    onProcessingComplete: (result) => {
      // Mock: Update extracted info when ID front is processed
      setExtractedInfo((prev) => ({
        ...prev,
        idFront: { ...result },
      }));
    },
  });

  const idBackUpload = useFileUpload<IdResultBack>({
    config: FILE_UPLOAD_CONFIGS.idBack,
    uploadFunction: (file, onUploadProgress) => processIdBack(sessionToken!, file, onUploadProgress),
    initialState: fileUploadStates.idBack,
    onProcessingComplete: (result) => {
      // Mock: Update extracted info when ID back is processed
      setExtractedInfo((prev) => ({
        ...prev,
        idBack: { ...result },
      }));
    },
  });

  const letterUpload = useFileUpload({
    config: FILE_UPLOAD_CONFIGS.letter,
    uploadFunction: (file, onUploadProgress) => processLetter(sessionToken!, file, onUploadProgress),
    initialState: fileUploadStates.letter,
  });

  const proofOfAddressUpload = useFileUpload({
    config: FILE_UPLOAD_CONFIGS.proofOfAddress,
    uploadFunction: (file, onUploadProgress) => processLetter(sessionToken!, file, onUploadProgress),
    initialState: fileUploadStates.proofOfAddress,
  });

  const pictureUpload = useFileUpload({
    config: FILE_UPLOAD_CONFIGS.picture,
    uploadFunction: (file, onUploadProgress) => processLetter(sessionToken!, file, onUploadProgress),
    initialState: fileUploadStates.picture,
  });

  // File uploads map for easy access
  const fileUploads: Record<FileType, ReturnType<typeof useFileUpload<any>>> = {
    paycheck: paycheckUpload,
    idFront: idFrontUpload,
    idBack: idBackUpload,
    letter: letterUpload,
    proofOfAddress: proofOfAddressUpload,
    picture: pictureUpload,
  };

  // Sync file upload states to context whenever they change
  useEffect(() => {
    const newStates: Record<FileType, any> = {} as Record<FileType, any>;
    Object.entries(fileUploads).forEach(([type, upload]) => {
      newStates[type as FileType] = upload.state;
    });
    setFileUploadStates(newStates);
  }, [
    paycheckUpload.state,
    idFrontUpload.state,
    idBackUpload.state,
    letterUpload.state,
    proofOfAddressUpload.state,
    pictureUpload.state,
  ]);

  // Calculate processing state
  const isProofOfAddressRequired = extractedInfo.idFront?.domicilio?.esta_completado === false;
  
  const requiredFiles: FileType[] = [
    ...(hasTalones ? ['paycheck' as FileType] : []),
    'idFront',
    'idBack',
    // 'picture',
    ...(isProofOfAddressRequired ? ['proofOfAddress' as FileType] : []),
  ];
  const completedRequiredFiles = requiredFiles.filter(
    (type) => fileUploads[type].state.status === 'completed'
  ).length;

  const isAnyProcessing = Object.values(fileUploads).some(
    (upload) => upload.state.status === 'processing'
  );
  console.log(isAnyProcessing)

  const allRequiredCompleted = completedRequiredFiles === requiredFiles.length;

  // Handle file selection
  const handleFileSelect = (type: FileType) => (file: File) => {
    fileUploads[type].handleFileSelect(file, (onUploadProgress) => fileUploads[type].uploadFunction(file, onUploadProgress));
     fileUploads[type].handleFileSelect
    form.setValue(type as keyof Step1FormData, file as File, {
      shouldValidate: true,
    });
  };

  // Handle file removal
  const handleFileRemove = (type: FileType) => () => {
    fileUploads[type].handleRemove();
    form.setValue(type as keyof Step1FormData, undefined as unknown as File, {
      shouldValidate: true,
    });
    
    // Reset extracted info related to this file
    if (type === 'paycheck') {
      setExtractedInfo((prev) => ({
        ...prev,
        paycheck: {
          ...Object.keys(prev.paycheck).reduce((acc, key) => ({ ...acc, [key]: null }), {}) as PaycheckResult,
        },
      }));
    } else if (type === 'idFront') {
      setExtractedInfo((prev) => ({
        ...prev,
        idFront: {
          ...Object.keys(prev.idFront).reduce((acc, key) => ({ ...acc, [key]: null }), {}) as typeof prev.idFront,
        },
      }));
    } else if (type === 'idBack') {
      setExtractedInfo((prev) => ({
        ...prev,
        idBack: {
          ...Object.keys(prev.idBack).reduce((acc, key) => ({ ...acc, [key]: null }), {}) as typeof prev.idBack,
        },
      }));
    }
  };

  // Handle extracted info changes (for editable fields)
  const handleExtractedInfoChange = (newData: typeof extractedInfo) => {
    setExtractedInfo(newData);
  };
  
  // Handle next step
  const handleNext = () => {
    if (allRequiredCompleted) {
      onNext(extractedInfo);
    }
  };

  return (
    <>
      {/* Section 1: Required Documents */}
      <div className="mb-8">
        <div className="mb-4">
          <h3 className="text-lg font-semibold text-foreground">Documentos Requeridos</h3>
          <p className="text-sm text-muted-foreground">Estos documentos son obligatorios para continuar</p>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <FileUpload
            config={FILE_UPLOAD_CONFIGS.idFront}
            state={idFrontUpload.state}
            onFileSelect={handleFileSelect('idFront')}
            onRemove={handleFileRemove('idFront')}
          />
          <FileUpload
            config={FILE_UPLOAD_CONFIGS.idBack}
            state={idBackUpload.state}
            onFileSelect={handleFileSelect('idBack')}
            onRemove={handleFileRemove('idBack')}
          />
          <FileUpload
            config={FILE_UPLOAD_CONFIGS.picture}
            state={pictureUpload.state}
            onFileSelect={handleFileSelect('picture')}
            onRemove={handleFileRemove('picture')}
          />
        </div>
      </div>

      {/* Section 2: Paycheck Documents */}
      <div className="mb-8">
        <div className="mb-4">
          <h3 className="text-lg font-semibold text-foreground">Talones de Pago</h3>
          <p className="text-sm text-muted-foreground mb-4">¿Tienes talones de pago para subir?</p>
          
          <div className="flex items-center gap-6 mb-4">
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="radio"
                checked={hasTalones === true}
                onChange={() => setHasTalones(true)}
                className="w-4 h-4 text-blue-600 border-gray-300 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">Sí, tengo talones</span>
            </label>
            <label className="flex items-center gap-2 cursor-pointer">
              <input
                type="radio"
                checked={hasTalones === false}
                onChange={() => setHasTalones(false)}
                className="w-4 h-4 text-blue-600 border-gray-300 focus:ring-blue-500"
              />
              <span className="text-sm text-gray-700">No tengo talones</span>
            </label>
          </div>
        </div>

        {hasTalones && (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <FileUpload
              config={FILE_UPLOAD_CONFIGS.paycheck}
              state={paycheckUpload.state}
              onFileSelect={handleFileSelect('paycheck')}
              onRemove={handleFileRemove('paycheck')}
            />
            <FileUpload
              config={FILE_UPLOAD_CONFIGS.letter}
              state={letterUpload.state}
              onFileSelect={handleFileSelect('letter')}
              onRemove={handleFileRemove('letter')}
            />
          </div>
        )}
      </div>

      {/* Section 3: Additional Documents */}
      {isProofOfAddressRequired && (
        <div className="mb-8">
          <div className="mb-4">
            <h3 className="text-lg font-semibold text-foreground">Documentos Adicionales</h3>
            <p className="text-sm text-muted-foreground">
              Se requiere comprobante de domicilio porque la dirección en tu INE está incompleta
            </p>
          </div>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <FileUpload
              config={FILE_UPLOAD_CONFIGS.proofOfAddress}
              state={proofOfAddressUpload.state}
              onFileSelect={handleFileSelect('proofOfAddress')}
              onRemove={handleFileRemove('proofOfAddress')}
              isRequired={true}
            />
          </div>
        </div>
      )}

      {/* Extracted Info Panel */}
      <ExtractedInfoPanel
        data={extractedInfo}
        onDataChange={handleExtractedInfoChange}
        isProcessing={isAnyProcessing}
        completedFiles={completedRequiredFiles}
        totalRequiredFiles={requiredFiles.length}
        showPaycheckInfo={hasTalones}
        className="mb-8"
      />

      {/* Navigation Buttons */}
      <div className="flex justify-end gap-4">
        <Button variant="outline" disabled>
          Anterior
        </Button>
        <Button
          onClick={handleNext}
          disabled={!allRequiredCompleted || isAnyProcessing}
          className="min-w-30"
        >
          {isAnyProcessing ? (
            <span className="flex items-center gap-2">
              <svg
                className="animate-spin h-4 w-4"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
              >
                <circle
                  className="opacity-25"
                  cx="12"
                  cy="12"
                  r="10"
                  stroke="currentColor"
                  strokeWidth="4"
                />
                <path
                  className="opacity-75"
                  fill="currentColor"
                  d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                />
              </svg>
              Procesando
            </span>
          ) : (
            'Siguiente'
          )}
        </Button>
      </div>
    </>
  );
}
