'use client';

import * as React from 'react';
import { cn } from '@/lib/utils';
import type { BannerInfo } from '@/types/file-upload';

interface AlertBannerProps {
  banner: BannerInfo | null;
  onDismiss?: () => void;
  className?: string;
}

// Icons
const AlertTriangleIcon = ({ className }: { className?: string }) => (
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
    <path d="m21.73 18-8-14a2 2 0 0 0-3.48 0l-8 14A2 2 0 0 0 4 21h16a2 2 0 0 0 1.73-3Z" />
    <path d="M12 9v4" />
    <path d="M12 17h.01" />
  </svg>
);

const AlertCircleIcon = ({ className }: { className?: string }) => (
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

export function AlertBanner({ banner, onDismiss, className }: AlertBannerProps) {
  if (!banner) return null;

  const isWarning = banner.type === 'warning';
  const isError = banner.type === 'error';

  return (
    <div
      className={cn(
        'relative flex items-start gap-3 rounded-lg border p-4 transition-all duration-300 animate-fade-in',
        isWarning && 'bg-yellow-50 border-yellow-200 text-yellow-800 dark:bg-yellow-950/30 dark:border-yellow-800/50 dark:text-yellow-200',
        isError && 'bg-red-50 border-red-200 text-red-800 dark:bg-red-950/30 dark:border-red-800/50 dark:text-red-200',
        className
      )}
      role="alert"
    >
      {/* Icon */}
      <div className="flex-shrink-0 mt-0.5">
        {isWarning && (
          <AlertTriangleIcon className="h-5 w-5 text-yellow-600 dark:text-yellow-400" />
        )}
        {isError && (
          <AlertCircleIcon className="h-5 w-5 text-red-600 dark:text-red-400" />
        )}
      </div>

      {/* Message */}
      <div className="flex-1 min-w-0">
        <p className="text-sm font-medium">
          {isWarning && 'Atención'}
          {isError && 'Error'}
        </p>
        <p className="text-sm mt-0.5 opacity-90">
          {Array.isArray(banner.message) ? banner.message.map((msg, index) => (
            <span key={index}>
              - {msg}
              {index < banner.message.length - 1 && <br />}
            </span>
          )) : banner.message} 
        </p>
      </div>

      {/* Dismiss button */}
      {onDismiss && (
        <button
          onClick={onDismiss}
          className={cn(
            'flex-shrink-0 rounded-md p-1 transition-colors',
            isWarning && 'hover:bg-yellow-200/50 dark:hover:bg-yellow-800/30',
            isError && 'hover:bg-red-200/50 dark:hover:bg-red-800/30'
          )}
          aria-label="Dismiss"
        >
          <XIcon className="h-4 w-4" />
        </button>
      )}
    </div>
  );
}
