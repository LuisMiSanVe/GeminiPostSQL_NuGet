> [Ver en ingles/See in english](https://github.com/LuisMiSanVe/GeminiPostSQL_NuGet/tree/main)

<img src="https://github.com/LuisMiSanVe/LuisMiSanVe/blob/main/Resources/GeminiPostSQL/GeminiPostSQLNuGet_banner.png" style="width: 100%; height: auto;" alt="GeminiPostSQL NuGet Banner">

# <img src="https://github.com/LuisMiSanVe/GeminiPostSQL_NuGet/blob/main/GeminiPostSQL/Resources/icon.ico" width="40" alt="Logo de GeminiPostSQL"> GeminiPostSQL NuGet | Paquete NuGet Asistido por IA para PostgreSQL
[![image](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://dotnet.microsoft.com/en-us/languages/csharp)
[![image](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet)
[![image](https://img.shields.io/badge/postgres-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![image](https://img.shields.io/badge/json-5E5C5C?style=for-the-badge&logo=json&logoColor=white)](https://www.newtonsoft.com/json)
[![image](https://img.shields.io/badge/Google%20Gemini-8E75B2?style=for-the-badge&logo=googlegemini&logoColor=white)](https://aistudio.google.com/app/apikey)
[![image](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visual%20studio&logoColor=white)](https://visualstudio.microsoft.com/)
[![NuGet](https://img.shields.io/badge/NuGet-%23004880.svg?style=for-the-badge&logo=nuget&logoColor=white)](https://www.nuget.org/)

>[!NOTE]
> Dale un vistazo a las otras versiones del programa:
>- [WinForms](https://github.com/LuisMiSanVe/GeminiPostSQL/tree/main)
>- [REST API](https://github.com/LuisMiSanVe/GeminiPostSQL_API/tree/main) 
>- [ChatBot](https://github.com/LuisMiSanVe/GeminiPostSQL_ChatBot/tree/main)
>- [Android](https://github.com/LuisMiSanVe/GeminiLiteSQL/tree/main)

Este paquete NuGet usa la IA de Google 'Gemini 2.0 Flash' para generar consultas a bases de datos PostgreSQL.  
La IA convierte lenguaje natural a consultas SQL usando un método con sus ventajas y desventajas.

## 📋 Prerequisitos

Para que el programa funcione, necesiatarás un servidor PostgreSQL y una clave de la API de Gemini.

> [!NOTE]  
> Yo usaré pgAdmin para montar el servidor PostgreSQL.

## 🛠️ Instalación

En caso de no tenerlo, deberás descargar [pgAdmin 4 desde su página ofical](https://www.pgadmin.org/download/).  
Ahora, crea el servidor y monta la base de datos con algunas tablas y valores.

Después, obten tu clave de la API de Gemini yendo aquí: [Google AI Studio](https://aistudio.google.com/app/apikey). Asegúrate de tener tu sesión de Google abierta, y entonces dale al botón que dice 'Crear clave de API' y sigue los pasos para crear tu proyecto de Google Cloud y conseguir tu clave de API. **Guárdala en algún sitio seguro**.  
Google permite el uso gratuito de esta API sin añadir ninguna forma de pago, pero con algunas limitaciones.

En Google AI Studio, puedes monitorizar el uso de la IA haciendo clic en 'Ver datos de uso' en la columna de 'Plan' en la tabla con todos tus proyectos. Recomiendo monitorizarla desde la pestaña de 'Cuota y límites del sistema' y ordenando por 'Porcentaje de uso actual', ya que es donde más información obtienes.

Ya tienes todo lo que necesitas para hacer funcionar el programa.  
Simplemente pon los datos que acabas de conseguir en el objeto Client.

## 📖 Sobre el paquete NuGet
Para usarlo, debes inicializar el [Client](https://github.com/LuisMiSanVe/GeminiPostSQL_NuGet/blob/main/GeminiPostSQL/GeminiPostSQLClient.cs) el cual tiene tres métodos:

**[GenerateSQL(string input):](https://github.com/LuisMiSanVe/GeminiPostSQL_NuGet/blob/main/GeminiPostSQL/GeminiPostSQLClient.cs#L115)**
Traduce lenguaje natural a sentencias SQL.
En caso que el parametro del Cliente 'mapOnGenerateSQL' este activado, si 'jsonMap' está vacio, mapeará la base de datos en un JSON automaticamente.
Devuelve la SQL generada en caso de éxito, en caso contrario devolverá un mensaje de error (todos empiezan con el texto '[ERROR]'). En caso que el parametro del Cliente 'runOnGenerateSQL' este activado, si tiene éxito, el métodod devolverá un JSON serializado de la tabla con el resultado final.

**[MapDatabase():](https://github.com/LuisMiSanVe/GeminiPostSQL_NuGet/blob/main/GeminiPostSQL/GeminiPostSQLClient.cs#L199)**
Mapea la base de datos en un JSON serializado.
Devuelve un booleano, dependiendo si el proceso fue exitoso o no, si lo es, el JSON se guardará en el parametro del Cliente 'jsonMap'.

**[RunQuery(string generatedSql):](https://github.com/LuisMiSanVe/GeminiPostSQL_NuGet/blob/main/GeminiPostSQL/GeminiPostSQLClient.cs#L274)**
Ejecuta la sentecia generada (o cualquiera) en el servidor de la base de datos.
Devuelve la tabla de resultados de la semtencia SQL generada. Si falla, devolverá una tabla con solo una columna llamada 'Error' con la descripcion del fallo. 

> [!TIP]
> Puedes probar sus funcionalidades en la [Demostración de prueba](https://github.com/LuisMiSanVe/GeminiPostSQL_NuGetTest/tree/main)

## 🚀 Lanzamientos
Una versión será lanzada solo cuando se cumplan los siguientes puntos:\
Nuevas funciones importantes y arreglos de fallos criticos causarán la salida inmediata de una nueva versión, mientras que otros cambios o arreglos menores deberán esperar una semana desde que se incluyeron en el repositorio antes de ser incluidos en la nueva versión, para que otros posibles cambios puedan ser añadidos también.
>[!NOTE]
>Estos posibles nuevos cambios no alargarán la espera de la salida de la nueva versión a más de una semana.

El número de la versión seguirá este formato: \
\[Añadido Importante\].\[Añadido Menor\].\[Arreglos de Errores\]

## 💻 Tecnologías usadas
- Lenguaje de programación: [C#](https://dotnet.microsoft.com/en-us/languages/csharp)
- Framework: [.Net](https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet) 8.0 Framework
- Paquetes NuGet:
  - [Npgsql](https://www.npgsql.org/) (8.0.5)
  - [RestSharp](https://restsharp.dev/) (112.1.0)
  - [Newtonsoft.Json](https://www.newtonsoft.com/json) (13.0.3)
- Otros:
  - [PostgreSQL](https://www.postgresql.org/) (16.3)
  - [pgAdmin 4](https://www.pgadmin.org/) (8.9)
  - Gemini API Key (2.0 Flash)
  - [FreeIcons](https://freeicons.io/) (Fuente original de los iconos, luego retocados por mí)
- IDE Recomendado: [Visual Studio](https://visualstudio.microsoft.com/) 2022
