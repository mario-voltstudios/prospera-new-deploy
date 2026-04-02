import { IdResult, IdResultBack } from '@/types/id-result'
import {apiClient} from './request.config'
import { PaycheckResult } from '@/types/paycheck-result'
import { AxiosProgressEvent } from 'axios'

const agentsApi = apiClient.create({ ...apiClient.defaults, baseURL: apiClient.defaults.baseURL + '/agent' })


type SessionResponse = {
    sessionToken: string
}

export const getSession = () => agentsApi.get<SessionResponse>('get-session').then(res => res.data)

export const processPaycheck = (sessionToken: string, file: File, onUploadProgress?: (progressEvent: AxiosProgressEvent) => void) => {
    const formData = new FormData()
    formData.append('file', file)
    formData.append('sessionToken', sessionToken)

    return agentsApi.post<PaycheckResult>(
        '/process/paycheck',
        formData,
        {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
            onUploadProgress: onUploadProgress,
        }
    ).then(res => res.data)
    .catch(err => {
        console.error('Error processing paycheck:', err)
        throw err
    })
}

export const processIdFront = (sessionToken: string, file: File, onUploadProgress?: (progressEvent: AxiosProgressEvent) => void) => {
    const formData = new FormData()
    formData.append('file', file)
    formData.append('sessionToken', sessionToken)

    return agentsApi.post<IdResult>(
        '/process/id-front',
        formData,
        {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
            onUploadProgress: onUploadProgress,
        }
    ).then(res => res.data)
    .catch(err => {
        console.error('Error processing id front:', err)
        throw err
    })
}

export const processIdBack = (sessionToken: string, file: File, onUploadProgress?: (progressEvent: AxiosProgressEvent) => void) => {
    const formData = new FormData()
    formData.append('file', file)
    formData.append('sessionToken', sessionToken)

    return agentsApi.post<IdResultBack>(
        '/process/id-back',
        formData,
        {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
            onUploadProgress: onUploadProgress,
        }
    ).then(res => res.data)
    .catch(err => {
        console.error('Error processing id back:', err)
        throw err
    })
}  

export const processLetter = (sessionToken: string, file: File, onUploadProgress?: (progressEvent: AxiosProgressEvent) => void) => {
    const formData = new FormData()
    formData.append('file', file)
    formData.append('sessionToken', sessionToken)

    return agentsApi.post<any>(
        '/process/letter',
        formData,
        {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
            onUploadProgress: onUploadProgress,
        }
    ).then(res => res.data)
    .catch(err => {
        console.error('Error uploading letter:', err)
        throw err
    })
}

export const processProofOfAddress = (sessionToken: string, file: File, onUploadProgress?: (progressEvent: AxiosProgressEvent) => void) => {
    const formData = new FormData()
    formData.append('file', file)
    formData.append('sessionToken', sessionToken)

    return agentsApi.post<any>(
        '/process/proof-of-address',
        formData,
        {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
            onUploadProgress: onUploadProgress,
        }
    ).then(res => res.data)
    .catch(err => {
        console.error('Error uploading proof of address:', err)
        throw err
    })
}

export const uploadPhoto = (sessionToken: string, file: File, onUploadProgress?: (progressEvent: AxiosProgressEvent) => void) => {
    const formData = new FormData()
    formData.append('file', file)
    formData.append('sessionToken', sessionToken)

    return agentsApi.post<any>(
        '/upload/photo',
        formData,
        {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
            onUploadProgress: onUploadProgress,
        }
    ).then(res => res.data)
    .catch(err => {
        console.error('Error uploading photo:', err)
        throw err
    })
}

