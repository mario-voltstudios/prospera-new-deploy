"use client";

import { useCallback, useState } from "react";
import type {
    FileUploadConfig,
    FileUploadState,
    FileUploadStatus,
} from "@/types/file-upload";
import { AxiosProgressEvent } from "axios";

const initialState: FileUploadState = {
    file: null,
    preview: null,
    status: "idle",
    progress: 0,
    errorMessage: undefined,
};

interface UseFileUploadOptions<T> {
    config: FileUploadConfig;
    uploadFunction: (
        file: File,
        onUploadProgress?: (progressEvent: AxiosProgressEvent) => void,
    ) => Promise<T>;
    onProcessingComplete?: (result: T) => void;
    simulateProcessing?: boolean;
    initialState?: FileUploadState;
}

export function useFileUpload<T>({
    config,
    uploadFunction,
    onProcessingComplete,
    simulateProcessing = true,
    initialState: providedInitialState,
}: UseFileUploadOptions<T>) {
    const [state, setState] = useState<FileUploadState>(providedInitialState || initialState);

    const validateFile = useCallback(
        (file: File): string | null => {
            // Check file size
            if (file.size > config.maxSize) {
                const maxSizeMB = config.maxSize / (1024 * 1024);
                return `File size exceeds ${maxSizeMB}MB limit`;
            }

            // Check file type
            const acceptedTypes = config.accept.split(",").map((t) => t.trim());
            const fileExtension = `.${
                file.name.split(".").pop()?.toLowerCase()
            }`;
            const isAccepted = acceptedTypes.some((type) => {
                if (type.startsWith(".")) {
                    return type.toLowerCase() === fileExtension;
                }
                // Handle MIME types
                return file.type.match(new RegExp(type.replace("*", ".*")));
            });

            if (!isAccepted) {
                return `Invalid file type. Accepted: ${config.accept}`;
            }

            return null;
        },
        [config],
    );

    const createPreview = useCallback((file: File): string | null => {
        if (file.type.startsWith("image/")) {
            return URL.createObjectURL(file);
        }
        return null;
    }, []);

    const simulateFileProcessing = useCallback(
        async (file: File): Promise<void> => {
            // Random delay between 2-4 seconds
            const delay = 2000 + Math.random() * 2000;
            await new Promise((resolve) => setTimeout(resolve, delay));
            throw new Error("Simulated processing error");
            onProcessingComplete?.({} as T);
        },
        [onProcessingComplete],
    );

    const handleFileSelect = useCallback(
        async (
            file: File,
            fn: (
                onUploadProgress: (progressEvent: AxiosProgressEvent) => void,
            ) => Promise<T>,
        ) => {
            // Validate file
            const error = validateFile(file);

            if (error) {
                setState({
                    file,
                    preview: createPreview(file),
                    status: "error",
                    progress: 0,
                    errorMessage: error,
                });
                return;
            }

            const preview = createPreview(file);

            // Set to uploading state briefly
            setState({
                file,
                preview,
                status: "uploading",
                progress: 0,
            });

            try {
                var result = await fn((progressEvent) => {
                    const progress = Math.round(
                        (progressEvent.loaded * 100) /
                            (progressEvent.total || 1),
                    );
                    setState((prev) => ({
                        ...prev,
                        progress,
                    }));

                    if (progress === 100) {
                        setState((prev) => ({
                            ...prev,
                            status: "processing",
                            progress: 100,
                        }));
                    }
                });

                setState((prev) => ({
                    ...prev,
                    status: "completed",
                }));
                
                onProcessingComplete?.(result);

            } catch (error) {
                console.error("File upload/processing error:", error);
                setState((prev) => ({
                    ...prev,
                    status: "error",
                    errorMessage: "Processing failed. Please try again.",
                }));
            }
        },
        [
            validateFile,
            createPreview,
            simulateProcessing,
            simulateFileProcessing,
        ],
    );

    const handleRemove = useCallback(() => {
        // Revoke object URL to prevent memory leaks
        if (state.preview) {
            URL.revokeObjectURL(state.preview);
        }
        setState(initialState);
    }, [state.preview]);

    const setStatus = useCallback(
        (status: FileUploadStatus, errorMessage?: string) => {
            setState((prev) => ({
                ...prev,
                status,
                errorMessage,
            }));
        },
        [],
    );

    const reset = useCallback(() => {
        if (state.preview) {
            URL.revokeObjectURL(state.preview);
        }
        setState(initialState);
    }, [state.preview]);

    return {
        state,
        uploadFunction,
        handleFileSelect,
        handleRemove,
        setStatus,
        reset,
    };
}
