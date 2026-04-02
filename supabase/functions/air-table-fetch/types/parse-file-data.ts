export type ParsedFileData = {
  EstadodeCuenta: {
    SECCION: {
      DetalleComisiones: {
        DetalleContenido: { DetalleF12: { ITEM: PaidReceiptType }[] };
      }[];
    }[];
  };
};

export type PaidReceiptType = {
  FechaDetF12?: number | string;
  PolizaDetF12?: number | string;
  EndosoDetF12?: number | string;
  VencimientoDetF12?: number | string;
  ModeloAnioDetF12?: number | string;
  LineaDetF12?: string | string;
  ConceptoDetF12?: string | string;
  AfectoDetF12?: string | string;
  PorcentajeDetF12?: number | string;
  PrimeNetaDetF12?: number | string;
  PrimaNetaDetF12?: number | string;
  RecargoDetF12?: number | string;
  PMATotalDetF12?: number | string;
  ComisionDetF12?: number | string;
};
