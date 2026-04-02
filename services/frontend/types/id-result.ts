import { Gender } from "./gender";

export interface IdResult {
    nombre: string;
    apellido_paterno: string;
    apellido_materno: string;
    fecha_nacimiento: Date | null;
    domicilio: Domicilio;
   
    curp: string;
    sexo: Gender;
}

export type Domicilio = {
    calle: string;
    numero_exterior: string;
    numero_interior?: string;
    colonia: string;
    municipio: string;
    estado: string;
    codigo_postal: string;  
    esta_completado: boolean;
}

export interface IdResultBack {
    idmex: string;
}