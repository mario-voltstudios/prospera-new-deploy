'use client';

import * as React from 'react';
import { cn } from '@/lib/utils';
import { Card } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import type { FileUploadState, FileUploadConfig } from '@/types/file-upload';

interface FileUploadProps {
  config: FileUploadConfig;
  state: FileUploadState;
  onFileSelect: (file: File) => void;
  onRemove: () => void;
  isRequired?: boolean;
  className?: string;
}

// Icons as inline SVGs for simplicity
const UploadIcon = ({ className }: { className?: string }) => (
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
    <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4" />
    <polyline points="17 8 12 3 7 8" />
    <line x1="12" y1="3" x2="12" y2="15" />
  </svg>
);

const FileIcon = ({ className }: { className?: string }) => (
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
    <polyline points="10 9 9 9 8 9" />
  </svg>
);

const CheckIcon = ({ className }: { className?: string }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2.5"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className}
  >
    <polyline points="20 6 9 17 4 12" />
  </svg>
);

const XIcon = ({ className }: { className?: string }) => (
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
    <line x1="18" y1="6" x2="6" y2="18" />
    <line x1="6" y1="6" x2="18" y2="18" />
  </svg>
);

const Spinner = ({ className }: { className?: string }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={cn('animate-spin', className)}
  >
    <path d="M21 12a9 9 0 1 1-6.219-8.56" />
  </svg>
);

const AlertIcon = ({ className }: { className?: string }) => (
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
    <circle cx="12" cy="12" r="10" />
    <line x1="12" y1="8" x2="12" y2="12" />
    <line x1="12" y1="16" x2="12.01" y2="16" />
  </svg>
);

export function FileUpload({
  config,
  state,
  onFileSelect,
  onRemove,
  isRequired,
  className,
}: FileUploadProps) {
  const inputRef = React.useRef<HTMLInputElement>(null);
  const [isDragging, setIsDragging] = React.useState(false);

  const handleDragOver = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(true);
  };

  const handleDragLeave = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(false);
  };

  const handleDrop = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(false);

    const files = e.dataTransfer.files;
    if (files.length > 0) {
      onFileSelect(files[0]);
    }
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (files && files.length > 0) {
      onFileSelect(files[0]);
    }
    // Reset input so the same file can be selected again
    if (inputRef.current) {
      inputRef.current.value = '';
    }
  };

  const handleClick = () => {
    if (state.status === 'idle' || state.status === 'error') {
      inputRef.current?.click();
    }
  };

  const isImage = state.file?.type.startsWith('image/');
  const isPdf = state.file?.type === 'application/pdf';

  const formatFileSize = (bytes: number) => {
    if (bytes < 1024) return `${bytes} B`;
    if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} KB`;
    return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
  };

  return (
    <Card
      className={cn(
        'relative overflow-hidden transition-all duration-300',
        'border-2 border-dashed',
        isDragging && 'border-primary bg-primary/5 scale-[1.02]',
        state.status === 'idle' && 'border-muted-foreground/25 hover:border-primary/50 cursor-pointer',
        state.status === 'completed' && 'border-green-500/50 bg-green-500/5',
        state.status === 'error' && 'border-red-500/50 bg-red-500/5',
        state.status === 'processing' && 'border-blue-500/50',
        className
      )}
      onDragOver={handleDragOver}
      onDragLeave={handleDragLeave}
      onDrop={handleDrop}
      onClick={handleClick}
    >
      <input
        ref={inputRef}
        type="file"
        accept={config.accept}
        onChange={handleFileChange}
        className="hidden"
        aria-label={`Upload ${config.label}`}
      />

      <div className="p-6 min-h-50 flex flex-col items-center justify-center">
        {/* Header with label and required badge */}
        <div className="absolute top-3 left-3 flex items-center gap-2">
          <span className="text-sm font-medium text-foreground">{config.label}</span>
          {(isRequired ?? config.required) ? (
            <Badge variant="secondary" className="text-xs">Requerido</Badge>
          ) : (
            <Badge variant="outline" className="text-xs">Opcional</Badge>
          )}
        </div>

        {/* Remove button when file is present */}
        {state.file && state.status !== 'processing' && (
          <Button
            variant="ghost"
            size="icon"
            className="absolute top-2 right-2 h-8 w-8 hover:bg-destructive/10 z-10"
            onClick={(e) => {
              e.stopPropagation();
              onRemove();
            }}
          >
            <XIcon className="h-4 w-4" />
          </Button>
        )}

        {/* Content based on state */}
        {state.status === 'idle' && !state.file && (
          <div className="flex flex-col items-center justify-center text-center mt-4">
            <div className="rounded-full bg-muted p-4 mb-4">
              <UploadIcon className="h-8 w-8 text-muted-foreground" />
            </div>
            <p className="text-sm text-muted-foreground mb-1">
              Arrastra y suelta o haz clic para subir
            </p>
            <p className="text-xs text-muted-foreground/70">
              {config.description}
            </p>
          </div>
        )}

        {/* File Preview */}
        {state.file && (
          <div className="flex flex-col items-center justify-center mt-4 w-full">
            {/* Thumbnail or File Icon */}
            <div className="relative mb-3">
              {isImage && state.preview ? (
                <div className="relative">
                  <img
                    src={state.preview}
                    alt={state.file.name}
                    className={cn(
                      'max-h-24 max-w-full rounded-lg object-cover shadow-md',
                      state.status === 'processing' && 'opacity-50'
                    )}
                  />
                </div>
              ) : isPdf ? (
                <div
                  className={cn(
                    'rounded-lg bg-red-100 dark:bg-red-900/30 p-4',
                    state.status === 'processing' && 'opacity-50'
                  )}
                >
                  <FileIcon className="h-12 w-12 text-red-600 dark:text-red-400" />
                </div>
              ) : (
                <div
                  className={cn(
                    'rounded-lg bg-muted p-4',
                    state.status === 'processing' && 'opacity-50'
                  )}
                >
                  <FileIcon className="h-12 w-12 text-muted-foreground" />
                </div>
              )}

              {/* Status Overlay */}
              {state.status === 'processing' && (
                <div className="absolute inset-0 flex items-center justify-center">
                  <div className="rounded-full bg-blue-500 p-2 shadow-lg animate-pulse">
                    <Spinner className="h-6 w-6 text-white" />
                  </div>
                </div>
              )}

              {state.status === 'completed' && (
                <div className="absolute -bottom-2 -right-2">
                  <div className="rounded-full bg-green-500 p-1.5 shadow-lg animate-in zoom-in-50 duration-300">
                    <CheckIcon className="h-4 w-4 text-white" />
                  </div>
                </div>
              )}

              {state.status === 'error' && (
                <div className="absolute -bottom-2 -right-2">
                  <div className="rounded-full bg-red-500 p-1.5 shadow-lg">
                    <AlertIcon className="h-4 w-4 text-white" />
                  </div>
                </div>
              )}
            </div>

            {/* File Info */}
            <p className="text-sm font-medium text-foreground truncate max-w-full px-2">
              {state.file.name}
            </p>
            <p className="text-xs text-muted-foreground">
              {formatFileSize(state.file.size)}
            </p>

            {/* Status Text */}
            {state.status === 'processing' && (
              <p className="text-xs text-blue-600 dark:text-blue-400 mt-2 flex items-center gap-1.5">
                <Spinner className="h-3 w-3" />
                Procesando...
              </p>
            )}

            {state.status === 'completed' && (
              <p className="text-xs text-green-600 dark:text-green-400 mt-2 flex items-center gap-1.5">
                <CheckIcon className="h-3 w-3" />
                Completado
              </p>
            )}

            {state.status === 'error' && state.errorMessage && (
              <p className="text-xs text-red-600 dark:text-red-400 mt-2 text-center px-2">
                {state.errorMessage}
              </p>
            )}
          </div>
        )}
      </div>
    </Card>
  );
}
