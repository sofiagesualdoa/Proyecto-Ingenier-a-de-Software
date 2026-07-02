# 🌟 Guía de Instalación - Sistema de Ventas EverGlow v1.0

¡Bienvenido a la guía oficial de instalación de **Sistema de Ventas EverGlow**! Sigue atentamente los pasos detallados a continuación para configurar la base de datos e instalar el sistema correctamente en tu equipo.

---

## 📋 Requisitos Previos

Antes de comenzar, asegúrate de cumplir con los siguientes requisitos:
- **Motor de Base de Datos:** Tener instalado **Microsoft SQL Server** (2019 o superior recomendado) y **SQL Server Management Studio (SSMS)**.
- **Espacio en Disco:** Se requieren al menos 24,5 MB de espacio libre para la aplicación, más el espacio correspondiente para el despliegue de la base de datos.

---

## 🚀 Proceso de Instalación Paso a Paso

### Paso 1: Restaurar la Base de Datos (Restore Database)
Antes de ejecutar el instalador del software, es obligatorio montar la estructura de datos en tu servidor local de SQL Server:

1. Ingresa al repositorio de GitHub del proyecto y descarga el archivo de respaldo de la base de datos: `EverGlow.bak` (o el nombre correspondiente de tu backup).
2. Abre **SQL Server Management Studio (SSMS)** y conéctate a tu instancia local.
3. Haz clic derecho sobre la carpeta **Databases (Bases de datos)** y selecciona **Restore Database... (Restaurar base de datos...)**.
4. En la ventana emergente, selecciona la opción **Device (Dispositivo)**, haz clic en los tres puntos (`...`) y busca el archivo `.bak` que descargaste desde GitHub.
5. Selecciona la base de datos destino, asegúrate de que los nombres de archivo sean correctos en la pestaña *Files* y haz clic en **OK** para finalizar la restauración.

---

### Paso 2: Seleccione la Carpeta de Destino
Una vez restaurada la base de datos, ejecuta nuestro instalador interactivo. La primera pantalla te solicitará elegir dónde debe instalarse el programa. Por defecto, se sugiere la ruta `E:\Program Files (x86)\Sistema de Ventas EverGlow`. Puedes usar el botón **Examinar...** para cambiarla o hacer clic en **Siguiente**.

![Paso 2 - Seleccione la Carpeta de Destino](image_45c21c.png)

### Paso 3: Seleccione las Tareas Adicionales
En la siguiente ventana, podrás elegir qué tareas adicionales deseas que se realicen durante la instalación. Te recomendamos marcar la opción **Crear un acceso directo en el escritorio** para facilitar el ingreso diario al sistema. Luego, haz clic en **Siguiente**.

![Paso 3 - Seleccione las Tareas Adicionales](image_45c220.png)

### Paso 4: Listo para Instalar
El asistente mostrará un resumen de la configuración elegida (Carpeta de destino y Tareas adicionales). Revisa que todo esté correcto y haz clic en el botón **Instalar**. 


![Paso 4 - Listo para Instalar](image_45c23b.png)

### Paso 5: Completando la instalación (Finalizar)
¡Felicidades! El programa ha completado la instalación con éxito en tu sistema. Puedes dejar marcada la casilla **Ejecutar Sistema de Ventas EverGlow** si deseas abrir la aplicación de inmediato y presionar el botón **Finalizar** para salir del asistente.

![Paso 5 - Finalizar](image_45c242.png)

---

## 🛠️ Solución de Problemas Comunes (FAQ)

* **Error de conexión al iniciar el sistema:** Verifica que el servicio de SQL Server esté corriendo y que la base de datos se haya restaurado con el nombre correcto requerido por el sistema.
* **Error de permisos al instalar:** Asegúrate de ejecutar el archivo instalador haciendo clic derecho y seleccionando **Ejecutar como administrador** para que las dependencias internas se registren de forma correcta en el sistema operativo.
* **Falta de espacio:** Verifica tener la disponibilidad de espacio requerida (24,5 MB) en la unidad de almacenamiento seleccionada.