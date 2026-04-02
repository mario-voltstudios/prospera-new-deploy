'use client';

import * as React from 'react';
import { cn } from '@/lib/utils';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';

interface EditableFieldProps {
  label: string;
  value: string | null;
  onChange: (value: string) => void;
  placeholder?: string;
  isLoading?: boolean;
  required?: boolean;
  error?: string;
  type?: string;
  disabled?: boolean;
  className?: string;
  maxLength?: number;
}

// Skeleton loader component
function Skeleton({ className }: { className?: string }) {
  return (
    <div
      className={cn(
        'animate-pulse rounded-md bg-muted',
        className
      )}
    />
  );
}

export function EditableField({
  label,
  value,
  onChange,
  placeholder = 'Not extracted yet',
  isLoading = false,
  required = false,
  error,
  type = 'text',
  disabled = false,
  className,
  maxLength,
}: EditableFieldProps) {
  return (
    <div className={cn('space-y-1.5', className)}>
      <Label className="text-xs font-medium text-muted-foreground uppercase tracking-wider">
        {label}
        {required && <span className="text-red-500 ml-1">*</span>}
      </Label>
      {isLoading ? (
        <Skeleton className="h-9 w-full" />
      ) : (
        <>
          <Input
            type={type}
            value={value ?? ''}
            onChange={(e) => onChange(e.target.value)}
            placeholder={placeholder}
            maxLength={maxLength}
            className={cn(
              'h-9 text-sm transition-colors',
              !value && 'text-muted-foreground italic',
              error && 'border-red-500'
            )}
            disabled={disabled} 
          />
          {error && <p className="text-xs text-red-500 mt-1">{error}</p>}
        </>
      )}
    </div>
  );
}
