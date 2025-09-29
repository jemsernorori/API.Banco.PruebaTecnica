# API Banco - Prueba Técnica Backend

Este proyecto es una **API de prueba para gestión de clientes, cuentas y transacciones bancarias**, desarrollada siguiendo **Clean Architecture** y principios **SOLID**.

## Tecnologías

- .NET 8
- C#
- Visual Studio 2022
- Sqlite
- Dapper
- XUnit + Moq para pruebas unitarias
- Swagger para documentación de endpoints

## Estructura del proyecto

- **API.Banco.Presentation** → capa de controllers y endpoints
- **API.Banco.Application** → casos de uso / handlers
- **API.Banco.Domain** → entidades, enums e interfaces
- **API.Banco.Infrastructure** → implementaciones de repositorios
- **API.Banco.Tests** → pruebas unitarias de los handlers

## Funcionalidades

1. **Clientes**
   - Crear cliente
   - Obtener cliente
2. **Cuentas**
   - Crear cuenta
   - Consultar saldo y detalles de la cuenta
   - Aplicar intereses
3. **Transacciones**
   - Crear transacción (depósitos, retiros, transferencias)
   - Obtener resumen de transacciones

## Cómo correr el proyecto

1. Abrir la solución en **Visual Studio 2022**
2. Restaurar paquetes NuGet
3. Seleccionar el proyecto `API.Banco.Presentation` como proyecto de inicio
4. Ejecutar la API 

## Pruebas Unitarias

Se utilizan **XUnit y Moq** para simular los repositorios y probar los handlers:

- compilar el proyecto
- clic derecho al proyecto `API.Banco.Tests` y seleccionar Ejecutar pruebas
