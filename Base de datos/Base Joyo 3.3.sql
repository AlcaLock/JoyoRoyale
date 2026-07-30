--use master
--drop database JoyoRoyale;
-- Crear base de datos

CREATE DATABASE JoyoRoyale;
GO

USE JoyoRoyale;
GO

-- Tabla Roles
CREATE TABLE Roles (
    ID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(50) NOT NULL
);

-- Tabla Usuarios
CREATE TABLE Usuarios (
    ID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(15) NOT NULL,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    FechaNacimiento DATE NOT NULL,
    Pais NVARCHAR(50) NOT NULL,
    Contrasena NVARCHAR(100) NOT NULL,
    RolID INT NOT NULL,
    FOREIGN KEY (RolID) REFERENCES Roles(ID)
);

-- Tabla Destinos
CREATE TABLE Destinos (
    ID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL
);

-- Tabla Puertos
CREATE TABLE Puertos (
    ID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Pais NVARCHAR(50) NOT NULL,
    DestinoID INT NOT NULL,
    FOREIGN KEY (DestinoID) REFERENCES Destinos(ID)
);

-- Tabla Barcos
CREATE TABLE Barcos (
    ID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255) NOT NULL,
    Capacidad INT NOT NULL,
	Imagen varbinary(max) NOT NULL
);

-- Tabla Habitaciones
CREATE TABLE Habitaciones (
    ID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    CapacidadMinima INT NOT NULL,
    CapacidadMaxima INT NOT NULL,
    Tamano FLOAT NOT NULL,
	Imagen varbinary(max) NOT NULL
);

-- Tabla BarcoHabitaciones (Relación muchos a muchos entre Barcos y Habitaciones)
CREATE TABLE BarcoHabitaciones (
    ID INT PRIMARY KEY IDENTITY,
    BarcoID INT NOT NULL,
    HabitacionID INT NOT NULL,
    CantidadDisponible INT NOT NULL,
    FOREIGN KEY (BarcoID) REFERENCES Barcos(ID),
    FOREIGN KEY (HabitacionID) REFERENCES Habitaciones(ID)
);

-- Tabla Cruceros
CREATE TABLE Cruceros (
    ID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
	Descripcion NVARCHAR(180) NOT NULL,
    Imagen varbinary(max) NOT NULL,
    Dias INT NOT NULL,
    BarcoID INT NOT NULL,
    FOREIGN KEY (BarcoID) REFERENCES Barcos(ID)
);

-- Tabla Itinerarios
CREATE TABLE Itinerarios (
    ID INT PRIMARY KEY IDENTITY,
    Dia INT NOT NULL,
    PuertoID INT NOT NULL,
    Descripcion NVARCHAR(255),
    CruceroID INT NOT NULL,
    FOREIGN KEY (PuertoID) REFERENCES Puertos(ID),
    FOREIGN KEY (CruceroID) REFERENCES Cruceros(ID)
);

-- Tabla FechasCruceros
CREATE TABLE FechasCruceros (
    ID INT PRIMARY KEY IDENTITY,
    FechaInicio DATE NOT NULL,
    FechaLimitePago DATE NOT NULL,
    CruceroID INT NOT NULL,
    FOREIGN KEY (CruceroID) REFERENCES Cruceros(ID)
);

-- Tabla PreciosHabitaciones
CREATE TABLE PreciosHabitaciones (
    ID INT PRIMARY KEY IDENTITY,
    Precio DECIMAL(10, 2) NOT NULL,
    HabitacionID INT NOT NULL,
    FechaCruceroID INT NOT NULL,
    FOREIGN KEY (HabitacionID) REFERENCES Habitaciones(ID),
    FOREIGN KEY (FechaCruceroID) REFERENCES FechasCruceros(ID)
);

-- Tabla Complementos
CREATE TABLE Complementos (
    ID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    Precio DECIMAL(10, 2) NOT NULL,
    TipoAplicacion NVARCHAR(50) NOT NULL -- Por camarote o por huésped
);

-- Tabla Reservas
CREATE TABLE Reservas (
    ID INT PRIMARY KEY IDENTITY,
    UsuarioID INT NOT NULL,
    CruceroID INT NOT NULL,
    Total DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(ID),
    FOREIGN KEY (CruceroID) REFERENCES Cruceros(ID)
);

-- Tabla ReservasHabitaciones (Relación entre Reservas y Habitaciones existentes)
CREATE TABLE ReservasHabitaciones (
    ID INT PRIMARY KEY IDENTITY,
    ReservaID INT NOT NULL,
    HabitacionID INT NOT NULL,
    CantidadPasajeros INT NOT NULL,
    FOREIGN KEY (ReservaID) REFERENCES Reservas(ID),
    FOREIGN KEY (HabitacionID) REFERENCES Habitaciones(ID)
);

-- Tabla Huespedes
CREATE TABLE Huespedes (
    ID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    Edad INT NOT NULL,
	Telefono nvarchar(50) NULL,
    DocumentoIdentidad NVARCHAR(50) NOT NULL,
    ReservaID INT NOT NULL,
    FOREIGN KEY (ReservaID) REFERENCES Reservas(ID)
);

-- Tabla ReservasComplementos
CREATE TABLE ReservasComplementos (
    ID INT PRIMARY KEY IDENTITY,
    ComplementoID INT NOT NULL,
    ReservaID INT NOT NULL,
    Cantidad INT NOT NULL,
    Total DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (ComplementoID) REFERENCES Complementos(ID),
    FOREIGN KEY (ReservaID) REFERENCES Reservas(ID)
);






-- Insertar Roles
INSERT INTO Roles (Nombre) VALUES ('Administrador'), ('Cliente');



-- Insertar Destinos
INSERT INTO Destinos (Nombre) VALUES ('Caribe'), ('Sudamérica'), ('Costa Oeste de Norteamérica');

-- Insertar Puertos
INSERT INTO Puertos (Nombre, Pais, DestinoID) 
VALUES ('Puerto de San Juan', 'Puerto Rico', 1),
       ('Prince George Wharf', 'Bahamas', 1),
       ('Montego Freeport', 'Jamaica', 1),
       ('Puerto de Buenos Aires', 'Argentina', 2),
       ('Puerto de Cartagena', 'Colombia', 2),
       ('Puerto de Valparaíso', 'Chile', 2),
       ('Port of San Diego', 'Estados Unidos', 3),
       ('Port of Vancouver', 'Canadá', 3),
       ('Puerto de Ensenada', 'México', 3),
	   ('En alta mar', 'Navegación', 1),
	   ('En alta mar', 'Navegación', 2),
	   ('En alta mar', 'Navegación', 3);


-- Insertar Barcos
INSERT INTO Barcos (Nombre, Descripcion, Capacidad, Imagen) 
VALUES ('Royal Star', 'Crucero de lujo con 10 pisos', 3000, 0x),
       ('Titanic II', 'Vive toda la experiencia del titanic', 2500, 0x),
       ('Pacific Explorer', 'Explorador de los mares del sur', 2600, 0x);


	    update Barcos set Imagen = CONVERT(varbinary(max),
	   (SELECT * FROM OPENROWSET(BULK 'C:\Imagenes\BarcoRoyaleStar.jpg', SINGLE_BLOB) AS image))
	   where id = 1;

	   update Barcos set Imagen = CONVERT(varbinary(max),
	   (SELECT * FROM OPENROWSET(BULK 'C:\Imagenes\BarcoTitanicII.jpg', SINGLE_BLOB) AS image))
	   where id = 2;

	    update Barcos set Imagen = CONVERT(varbinary(max),
	   (SELECT * FROM OPENROWSET(BULK 'C:\Imagenes\BarcoPacificExplorer.jpg', SINGLE_BLOB) AS image))
	   where id = 3;
	   
	   -- Insertar Habitaciones
INSERT INTO Habitaciones (Nombre, Descripcion, CapacidadMinima, CapacidadMaxima, Tamano, Imagen) 
VALUES ('Suite Presidencial', 'Habitación de lujo con jacuzzi', 1, 4, 50.5, 0x),
       ('Cabina Estándar', 'Cabina cómoda con vista al mar', 1, 2, 20.0, 0x),
       ('Cabina Familiar', 'Espacio ideal para familias', 2, 5, 35.0, 0x);

	  	    update Habitaciones set Imagen = CONVERT(varbinary(max),
	   (SELECT * FROM OPENROWSET(BULK 'C:\Imagenes\HabitacionSuitePresidencial.jpg', SINGLE_BLOB) AS image))
	   where id = 1;

	   update Habitaciones set Imagen = CONVERT(varbinary(max),
	   (SELECT * FROM OPENROWSET(BULK 'C:\Imagenes\HabitacionCabinaEstandar.jpg', SINGLE_BLOB) AS image))
	   where id = 2;

	    update Habitaciones set Imagen = CONVERT(varbinary(max),
	   (SELECT * FROM OPENROWSET(BULK 'C:\Imagenes\HabitacionCabinaFamiliar.jpg', SINGLE_BLOB) AS image))
	   where id = 3;



-- Insertar Complementos
INSERT INTO Complementos (Nombre, Descripcion, Precio, TipoAplicacion) 
VALUES ('WiFi Premium', 'Internet de alta velocidad', 50.00, 'Por camarote'),
       ('Cena Especial', 'Cena gourmet con chef exclusivo', 120.00, 'Por camarote');





	  




