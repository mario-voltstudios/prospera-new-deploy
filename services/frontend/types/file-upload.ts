import { IdResult, IdResultBack } from "./id-result";
import { PaycheckResult } from "./paycheck-result";

export type FileUploadStatus = 
  | 'idle' 
  | 'uploading' 
  | 'processing' 
  | 'completed' 
  | 'error';

export interface FileUploadState {
  file: File | null;
  preview: string | null;
  status: FileUploadStatus;
  progress: number;
  errorMessage?: string;
}

// Extracted info from ID Front
export interface IdFrontInfo {
  nombre: string | null;
  domicilio: string | null;
  curp: string | null;
  fechaNacimiento: string | null;
  sexo: string | null;
}

// Extracted info from ID Back
export interface IdBackInfo {
  validationCode: string | null;
}

// Combined extracted info
export interface ExtractedInfo {
  paycheck: PaycheckResult;
  idFront: IdResult;
  idBack: IdResultBack;
}

// Banner types for warnings/errors
export type BannerType = 'warning' | 'error';

export interface BannerInfo {
  type: BannerType;
  message: string | string[];
}

export type FileType = 'paycheck' | 'idFront' | 'idBack' | 'letter' | 'proofOfAddress' | 'picture';

export interface FileUploadConfig {
  type: FileType;
  label: string;
  description: string;
  required: boolean;
  accept: string;
  maxSize: number; // in bytes
}

export const FILE_UPLOAD_CONFIGS: Record<FileType, FileUploadConfig> = {
  paycheck: {
    type: 'paycheck',
    label: 'Talones de pago',
    description: 'Sube tu talón de pago más reciente (PDF o imagen)',
    required: true,
    accept: '.pdf,.jpg,.jpeg,.png',
    maxSize: 10 * 1024 * 1024, // 10MB
  },
  idFront: {
    type: 'idFront',
    label: 'INE Frente',
    description: 'Lado frontal de tu credencial de elector',
    required: true,
    accept: '.jpg,.jpeg,.png',
    maxSize: 5 * 1024 * 1024, // 5MB
  },
  idBack: {
    type: 'idBack',
    label: 'INE Reverso',
    description: 'Lado posterior de tu credencial de elector',
    required: true,
    accept: '.jpg,.jpeg,.png',
    maxSize: 5 * 1024 * 1024, // 5MB
  },
  letter: {
    type: 'letter',
    label: 'Carta de instrucciones',
    description: 'Carta de apoyo opcional (PDF o imagen)',
    required: false,
    accept: '.pdf,.jpg,.jpeg,.png',
    maxSize: 10 * 1024 * 1024, // 10MB
  },
  proofOfAddress: {
    type: 'proofOfAddress',
    label: 'Comprobante de Domicilio',
    description: 'Recibo de luz, agua o teléfono reciente (PDF o imagen)',
    required: false,
    accept: '.pdf,.jpg,.jpeg,.png',
    maxSize: 10 * 1024 * 1024, // 10MB
  },
  picture: {
    type: 'picture',
    label: 'Fotografía',
    description: 'Foto tipo selfie o retrato reciente',
    required: true,
    accept: '.jpg,.jpeg,.png',
    maxSize: 5 * 1024 * 1024, // 5MB
  },
};
