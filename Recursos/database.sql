CREATE SEQUENCE public.usuario_id_seq;

ALTER SEQUENCE public.usuario_id_seq
    OWNER TO postgres;

CREATE SEQUENCE public.compra_id_seq;

ALTER SEQUENCE public.compra_id_seq
    OWNER TO postgres;

CREATE SEQUENCE public.detalle_compra_id_seq;

ALTER SEQUENCE public.detalle_compra_id_seq
    OWNER TO postgres;

CREATE SEQUENCE public.detalle_venta_id_seq;

ALTER SEQUENCE public.detalle_venta_id_seq
    OWNER TO postgres;

CREATE SEQUENCE public.producto_id_seq;

ALTER SEQUENCE public.producto_id_seq
    OWNER TO postgres;

CREATE SEQUENCE public.venta_id_seq;

ALTER SEQUENCE public.venta_id_seq
    OWNER TO postgres;

/*CREACION DE TABLA USUARIO*/
CREATE TABLE public.usuario
(
    id integer NOT NULL DEFAULT nextval('usuario_id_seq'::regclass),
    nombre text COLLATE pg_catalog."default" NOT NULL,
    dpi text COLLATE pg_catalog."default",
    usuario text COLLATE pg_catalog."default" NOT NULL,
    "contraseña" text COLLATE pg_catalog."default" NOT NULL,
    administrador boolean NOT NULL,
    borrado boolean DEFAULT false,
    CONSTRAINT usuario_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.usuario
    OWNER to postgres;

/*CREACION DE TABLA PRODUCTO*/
CREATE TABLE public.producto
(
    id integer NOT NULL DEFAULT nextval('producto_id_seq'::regclass),
    codigo_barra text COLLATE pg_catalog."default",
    nombre text COLLATE pg_catalog."default" NOT NULL,
    existencia integer NOT NULL,
    precio_venta numeric(7,2) NOT NULL,
    borrado boolean DEFAULT false,
    CONSTRAINT producto_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.producto
    OWNER to postgres;

/*CREACION DE TABLA VENTA*/
CREATE TABLE public.venta
(
    id integer NOT NULL DEFAULT nextval('venta_id_seq'::regclass),
    comprobante text COLLATE pg_catalog."default" NOT NULL,
    fecha date NOT NULL,
    id_usuario integer,
    CONSTRAINT venta_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.venta
    OWNER to postgres;

/*CREACION DE TABLA DETALLE_VENTA*/
CREATE TABLE public.detalle_venta
(
    id integer NOT NULL DEFAULT nextval('detalle_venta_id_seq'::regclass),
    id_venta integer NOT NULL,
    id_producto integer NOT NULL,
    cantidad double precision NOT NULL,
    precio double precision NOT NULL,
    CONSTRAINT detalle_venta_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.detalle_venta
    OWNER to postgres;

/*CREACION DE TABLA COMPRA*/
CREATE TABLE public.compra
(
    id integer NOT NULL DEFAULT nextval('compra_id_seq'::regclass),
    fecha date NOT NULL,
    id_usuario integer NOT NULL,
    CONSTRAINT compra_pkey PRIMARY KEY (id),
    CONSTRAINT fk_usuario FOREIGN KEY (id_usuario)
        REFERENCES public.usuario (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.compra
    OWNER to postgres;

/*CREACION DE TABLA DETALLE_COMPRA*/
CREATE TABLE public.detalle_compra
(
    id integer NOT NULL DEFAULT nextval('detalle_compra_id_seq'::regclass),
    id_compra integer NOT NULL,
    id_producto integer NOT NULL,
    cantidad double precision NOT NULL,
    precio_compra double precision NOT NULL,
    CONSTRAINT detalle_compra_pkey PRIMARY KEY (id),
    CONSTRAINT fk_compra FOREIGN KEY (id_compra)
        REFERENCES public.compra (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_producto FOREIGN KEY (id_producto)
        REFERENCES public.producto (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.detalle_compra
    OWNER to postgres;

CREATE OR REPLACE FUNCTION descontar() RETURNS TRIGGER as $descontar$
declare begin
UPDATE producto set existencia = existencia - NEW.cantidad WHERE producto.id = NEW.id_producto;
return null;
end;
$descontar$ LANGUAGE plpgsql;

CREATE TRIGGER descontar_inventario after insert
on detalle_venta for each row
execute procedure descontar();

/*--------------------------------------------------------------------*/
CREATE OR REPLACE FUNCTION proc_datos_factura(p_comprobante text)
RETURNS table(id int, comprobante text, fecha date, usuario text, total float) AS
$BODY$
	DECLARE
	BEGIN
		RETURN QUERY 
		SELECT V.id, V.comprobante, V.fecha, U.usuario, SUM(DV.precio * DV.cantidad) total
		FROM venta V 
		INNER JOIN detalle_venta DV ON V.id = DV.id_venta
		INNER JOIN usuario U ON V.id_usuario = U.id
		WHERE V.comprobante = p_comprobante
		GROUP BY V.id, V.comprobante, V.fecha, U.usuario;
	END;
$BODY$
LANGUAGE plpgsql VOLATILE;
/*----------------------------------------------------------------------*/
CREATE OR REPLACE FUNCTION proc_detalle_factura(p_comprobante text)
RETURNS table(nombre text, cantidad float, precio float, total float) AS
$BODY$
	DECLARE
		reg RECORD;
	BEGIN
		FOR REG IN 
		SELECT P.nombre, DV.cantidad, DV.precio, (DV.cantidad * DV.precio) total
		FROM detalle_venta DV INNER JOIN producto P
		ON DV.id_producto = P.id INNER JOIN venta V
		ON V.id = DV.id_venta
		AND V.comprobante = p_comprobante 
		LOOP
			nombre := reg.nombre;
			cantidad := reg.cantidad;
			precio := reg.precio;
			total := reg.total;
			RETURN NEXT;
		END LOOP;
		RETURN;
	END;
$BODY$
LANGUAGE plpgsql VOLATILE;
/*-------------------------------------------------------------*/
CREATE OR REPLACE FUNCTION proc_cantidad_productos_vendidos(p_fecha_inicial date, p_fecha_final date)
RETURNS table(nombre text, cantidad float, precio float, total float) AS
$BODY$
	DECLARE
		reg RECORD;
	BEGIN
		FOR REG IN 
		SELECT A.nombre, A.cantidad, A.precio, (A.precio * A.cantidad) total 
		FROM
			(SELECT P.nombre, DV.precio, sum(DV.cantidad) cantidad
			FROM producto P, detalle_venta DV, venta V
			WHERE V.id = DV.id_venta AND P.id = DV.id_producto
			AND V.fecha BETWEEN p_fecha_inicial AND p_fecha_final
			GROUP BY P.nombre, DV.precio) A
		LOOP
			nombre := reg.nombre;
			cantidad := reg.cantidad;
			precio := reg.precio;
			total := reg.total;
			RETURN NEXT;
		END LOOP;
		RETURN;
	END;
$BODY$
LANGUAGE plpgsql VOLATILE;
/*---------------------------------------------------------------*/
CREATE OR REPLACE FUNCTION proc_cantidad_ventas_por_dia(p_fecha_inicial date, p_fecha_final date)
RETURNS table(fecha date, ventas float, total float) AS
$BODY$
	DECLARE
		reg RECORD;
	BEGIN
		FOR REG IN 
		SELECT V.fecha, A.ventas, SUM(DV.cantidad * DV.precio) total
		FROM venta V, detalle_venta DV, (
			SELECT VE.fecha, COUNT(*) ventas
			FROM venta VE
			GROUP BY VE.fecha) A
		WHERE V.id = DV.id_venta AND V.fecha = A.fecha
		AND V.fecha BETWEEN p_fecha_inicial AND p_fecha_final
		GROUP BY V.fecha, A.ventas
		ORDER BY V.fecha DESC
		LOOP
			fecha := reg.fecha;
			ventas := reg.ventas;
			total := reg.total;
			RETURN NEXT;
		END LOOP;
		RETURN;
	END;
$BODY$
LANGUAGE plpgsql VOLATILE;
/*-----------------------------------------------------*/

CREATE OR REPLACE FUNCTION aumentar() RETURNS TRIGGER as $aumentar$
declare begin
UPDATE producto set existencia = existencia + NEW.cantidad WHERE producto.id = NEW.id_producto;
return null;
end;
$aumentar$ LANGUAGE plpgsql;

CREATE TRIGGER aumentar_inventario after insert
on detalle_compra for each row
execute procedure aumentar();

/*-------------------------------------------*/
INSERT INTO public.usuario(
	nombre, dpi, usuario, "contraseña", administrador, borrado)
	VALUES ('admin', '1515', 'admin', 'admin', true, false);

