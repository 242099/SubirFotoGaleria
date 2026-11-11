-- Crear la base de datos
CREATE DATABASE FotosAppDb;
GO

-- Usar la base de datos creada
USE FotosAppDb;
GO

-- Crear la tabla para almacenar las fotografías
CREATE TABLE Fotografias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(255) NOT NULL,
    ImageData VARBINARY(MAX) NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO


select * from Fotografias;