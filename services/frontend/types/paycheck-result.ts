import { HiringType } from "./hiringType";
import { Delegations } from "./delegations";

export interface PaycheckResult  {
    matricula: string;
    RFC: string;
    nombre: string;
    curp: string;
    tipo_de_contratacion: HiringType | null;
    clave_est_org: string;
    type: Delegations | null;
    percepciones: DateEntry[];
    deducciones: DateEntry[];
    observaciones: Observation[];
    contains_gnp_policy: boolean;
}

export interface DateEntry {
    concepto: string;
    descripcion: string;
    importe: number;
}

export interface Observation {
    concepto: string;
    importe: number;
    vencimento: string;
    unidades: string;
    num_control: string;
    observaciones: string;
}