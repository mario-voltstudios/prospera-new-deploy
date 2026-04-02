ALTER TABLE polices
ADD COLUMN poliza_url text GENERATED ALWAYS AS (
	'https://prospera-data.s3.us-east-2.amazonaws.com/policy-assets/' || "POLIZA" || '/' || "POLIZA" || '.pdf'
) STORED;

ALTER TABLE polices
ADD COLUMN solicitude_url text GENERATED ALWAYS AS (
	'https://prospera-data.s3.us-east-2.amazonaws.com/policy-assets/' || "POLIZA" || '/' || "POLIZA" || '-request.pdf'
) STORED;

ALTER TABLE polices
ADD COLUMN ine_url text GENERATED ALWAYS AS (
	'https://prospera-data.s3.us-east-2.amazonaws.com/policy-assets/' || "POLIZA" || '/' || "POLIZA" || '-ine.pdf'
) STORED;