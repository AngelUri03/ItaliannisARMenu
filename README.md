# Italianni's AR Menu

Aplicación móvil desarrollada en Unity para simular un menú interactivo con Realidad Aumentada para restaurante.  
El proyecto está enfocado en una experiencia tipo kiosko/tablet, donde el cliente puede explorar el menú, consultar información detallada de los platillos, visualizar modelos 3D y observar productos en Realidad Aumentada mediante un marcador de mesa.

---

## Descripción del proyecto

**Italianni's AR Menu** es un prototipo funcional para Android que transforma un menú tradicional en una experiencia visual, interactiva y comercial.

La aplicación permite que el usuario navegue por un menú digital, seleccione platillos, revise información detallada y visualice modelos 3D sobre un marcador físico usando Realidad Aumentada.

El objetivo principal es mejorar la experiencia del cliente dentro del restaurante, reducir la indecisión al ordenar y presentar los platillos de una forma más atractiva, moderna y diferenciada.

---

## Temática del proyecto

El proyecto pertenece principalmente a la temática de **Publicidad**, ya que busca mejorar la presentación visual de productos dentro de un restaurante y apoyar la decisión de compra del cliente mediante tecnología interactiva.

También se relaciona con **estilo de vida**, porque mejora la experiencia de consumo dentro del restaurante mediante una interacción más visual, moderna e intuitiva.

---

## Problema que resuelve

Los menús tradicionales presentan varias limitaciones:

- No muestran claramente el tamaño real o aproximado de los platillos.
- No permiten visualizar el producto antes de ordenar.
- La descripción textual puede no ser suficiente para todos los clientes.
- Puede existir indecisión al momento de elegir.
- La experiencia de menú suele ser poco diferenciada frente a otros restaurantes.

Este proyecto propone una solución mediante una app en tablet que combina menú digital, imágenes, modelos 3D y Realidad Aumentada.

---

## Solución propuesta

La solución consiste en una aplicación Android desarrollada en Unity, pensada para ejecutarse en una tablet colocada en mesa.

El cliente puede:

- Explorar el menú digital.
- Consultar platillos por categoría.
- Ver imágenes del producto.
- Revisar ingredientes, descripción, información nutrimental y alérgenos.
- Visualizar modelos 3D.
- Entrar a una experiencia de Realidad Aumentada.
- Ver el platillo sobre un marcador físico de mesa.
- Rotar y escalar el modelo.
- Cambiar entre platillos dentro de la vista AR.
- Agregar productos a una orden.
- Revisar y confirmar la orden.

---

## Tecnologías utilizadas

- Unity 6
- C#
- UI Toolkit
- UXML
- USS
- Vuforia Engine
- Android Build Support
- Modelos 3D en formato FBX
- Imágenes PNG/JPG para productos
- RenderTexture para visor 3D
- Git y GitHub para control de versiones
- Tripo AI para generación de modelos 3D
- ChatGPT Pro como apoyo para generación visual

---

## Plataforma objetivo

La aplicación fue desarrollada para Android y optimizada visualmente para tablet en orientación horizontal.

Dispositivo de prueba principal:

```text
Samsung Galaxy Tab S10 FE
```

La aplicación también puede ejecutarse en otros dispositivos Android compatibles, aunque el diseño fue ajustado principalmente para tablet.

---

## Estructura general del repositorio

```text
ItaliannisARMenu/
├── Assets/
│   └── _Project/
│       ├── Icons/
│       ├── Models/
│       ├── Prefabs/
│       ├── RenderTextures/
│       ├── Resources/
│       ├── Scenes/
│       ├── ScriptableObjects/
│       ├── Scripts/
│       └── UI/
├── Entregables/
│   ├── Build/
│   ├── Documentacion/
│   ├── Marcador/
│   └── Videos/
├── Packages/
├── ProjectSettings/
├── .gitignore
└── README.md
```

---

## Estructura de entregables

```text
Entregables/
├── Build/
│   └── APK disponible en GitHub Releases
├── Documentacion/
│   ├── 01_Manual_Usuario.pdf
│   ├── 02_Manual_Tecnico.pdf
│   ├── 03_Analisis_Costos_Viabilidad.pdf
│   └── 04_Determinacion_Precio.pdf
├── Marcador/
│   └── MarcadorItaliannis.jpg
└── Videos/
    └── Demo_App.mp4
```

> Nota: el APK no se sube directamente al repositorio porque supera el límite de tamaño permitido por GitHub.  
> El archivo instalable se encuentra disponible en la sección **Releases** del repositorio.

---

## APK final

El APK final se encuentra en la sección:

```text
GitHub → Releases → Italianni's AR Menu v1.0.0
```

Archivo:

```text
ItaliannisAR.apk
```

---

## Marcador de Realidad Aumentada

Para utilizar la experiencia AR se debe imprimir o visualizar el marcador ubicado en:

```text
Entregables/Marcador/MarcadorItaliannis.jpg
```

La aplicación utiliza este marcador para reconocer la superficie de mesa y colocar el modelo 3D del platillo mediante Vuforia.

Recomendaciones para el marcador:

- Imprimirlo en buena calidad.
- Evitar reflejos fuertes.
- Mantenerlo visible dentro de la cámara.
- Usarlo sobre una superficie plana.
- Tener buena iluminación.

---

## Requisitos para ejecutar el APK

- Dispositivo Android compatible.
- Cámara funcional.
- Permisos de cámara activados.
- Marcador de RA disponible.
- Buena iluminación.
- Espacio suficiente para instalar la aplicación.

---

## Requisitos para abrir el proyecto en Unity

- Unity 6 LTS o versión compatible.
- Android Build Support instalado.
- Vuforia Engine configurado.
- Visual Studio o editor compatible con C#.
- Sistema operativo Windows recomendado.
- Dispositivo Android para pruebas reales.

---

## Escenas principales

El proyecto utiliza dos escenas principales:

```text
01_MenuKiosk
02_ARExperience
```

### 01_MenuKiosk

Contiene la experiencia principal del menú digital:

- Pantalla de bienvenida.
- Menú principal.
- Cards de platillos.
- Búsqueda y categorías.
- Panel de detalle.
- Modal de información completa.
- Visor de modelo 3D.
- Orden de mesa.
- Confirmación de orden.

### 02_ARExperience

Contiene la experiencia de Realidad Aumentada:

- Cámara AR.
- Reconocimiento de marcador con Vuforia.
- Visualización de modelo 3D sobre el marcador.
- Controles de rotación.
- Controles de escala.
- Vista grande.
- Cambio entre platillos.
- Agregar platillo a la orden desde RA.
- Estado visual de tracking.

---

## Flujo de uso de la aplicación

1. Abrir la aplicación.
2. Entrar al menú digital.
3. Explorar los platillos por categoría.
4. Seleccionar un platillo.
5. Consultar su información detallada.
6. Ver su imagen y datos generales.
7. Abrir el visor de modelo 3D.
8. Entrar a la experiencia de Realidad Aumentada.
9. Apuntar la cámara al marcador de mesa.
10. Visualizar el platillo en 3D sobre el marcador.
11. Rotar, escalar o cambiar de platillo.
12. Agregar el producto a la orden.
13. Revisar la orden.
14. Confirmar la orden.

---

## Funcionalidades implementadas

### Menú digital

- Navegación por categorías.
- Filtros de platillos.
- Búsqueda de productos.
- Cards visuales con información resumida.
- Panel lateral de detalle.
- Imágenes de platillos.
- Botón para ver más información.
- Botón para abrir experiencia AR.
- Botón para agregar a la orden.

### Detalle de platillo

- Imagen del producto.
- Nombre del platillo.
- Categoría y subcategoría.
- Porción.
- Calorías.
- Etiquetas.
- Descripción.
- Ingredientes.
- Información nutrimental.
- Alérgenos.
- Modelo 3D integrado.

### Visor 3D

- Visualización de modelo 3D dentro del menú.
- Uso de RenderTexture.
- Cámara de preview.
- Luz dedicada para previsualización.
- Rotación automática del modelo.

### Realidad Aumentada

- Reconocimiento de marcador de mesa.
- Instanciación del modelo 3D correspondiente al platillo.
- Visualización del platillo sobre el marcador.
- Rotación izquierda y derecha.
- Escalado del modelo.
- Reset de escala y rotación.
- Vista grande.
- Cambio al platillo anterior.
- Cambio al siguiente platillo.
- Consulta de información dentro de RA.
- Agregar a orden desde RA.

### Orden

- Agregar productos desde menú.
- Agregar productos desde RA.
- Revisar productos agregados.
- Aumentar o disminuir cantidades.
- Eliminar productos.
- Vaciar orden.
- Confirmar orden.

### Confirmación

- Pantalla final de orden confirmada.
- Resumen de mesa.
- Total de productos.
- Estado de confirmación.
- Opciones para nueva orden, volver al menú o volver al inicio.

---

## Estructura técnica principal

### Controladores de UI

```text
Assets/_Project/Scripts/UI/Controllers/
├── KioskSceneController.cs
├── WelcomeViewController.cs
├── MenuHomeViewController.cs
├── OrderViewController.cs
├── ConfirmationViewController.cs
├── RecommendationsViewController.cs
└── ARExperienceController.cs
```

### Núcleo de datos y estado

```text
Assets/_Project/Scripts/UI/Core/
├── DishData.cs
├── MenuRepository.cs
├── KioskAppState.cs
├── RuntimeAppState.cs
└── AppSceneNames.cs
```

### Sistema AR

```text
Assets/_Project/Scripts/AR/
└── ARDishModelCatalog.cs
```

### Sistema de preview 3D

```text
Assets/_Project/Scripts/UI/Preview/
└── DishModelPreviewRenderer.cs
```

---

## Recursos principales

### Modelos 3D

```text
Assets/_Project/Models/AR_Dishes/
```

Contiene los modelos base en formato FBX y sus texturas.

### Prefabs de platillos

```text
Assets/_Project/Prefabs/AR_Dishes/
```

Contiene los prefabs usados por Unity para instanciar los modelos en el visor 3D y en Realidad Aumentada.

### Imágenes de platillos

```text
Assets/_Project/Resources/DishImages/
```

Contiene las imágenes PNG utilizadas dentro del menú, detalle, orden y otras vistas.

### Catálogo AR

```text
Assets/_Project/ScriptableObjects/ARDishModelCatalog.asset
```

Relaciona cada platillo con su prefab correspondiente mediante su identificador.

---

## Cómo agregar un nuevo platillo

1. Abrir `MenuRepository.cs`.
2. Crear o agregar el platillo dentro de la lista correspondiente.
3. Asignar un `dishId` único.
4. Agregar su imagen PNG en:

```text
Assets/_Project/Resources/DishImages/
```

5. El nombre de la imagen debe coincidir con el `dishId`.

Ejemplo:

```text
dishId: pizza_peperoni
imagen: pizza_peperoni.png
```

6. Agregar el modelo 3D como prefab en:

```text
Assets/_Project/Prefabs/AR_Dishes/
```

7. Registrar el prefab en:

```text
ARDishModelCatalog.asset
```

---

## Cómo generar el APK

1. Abrir el proyecto en Unity.
2. Verificar que las escenas estén agregadas en Build Settings:

```text
01_MenuKiosk
02_ARExperience
```

3. Seleccionar plataforma Android.
4. Revisar permisos de cámara.
5. Verificar configuración de Vuforia.
6. Ir a:

```text
File → Build Settings → Build
```

7. Guardar el APK fuera de carpetas ignoradas por Git.
8. Probar en dispositivo Android real.

---

## Evidencia de funcionamiento

El video de demostración se encuentra en:

```text
Entregables/Videos/Demo_App.mp4
```

La demostración incluye:

- Navegación del menú.
- Consulta de detalles.
- Visualización de modelos 3D.
- Uso de Realidad Aumentada.
- Agregar productos a la orden.
- Confirmación final.

---

## Estado final del proyecto

El proyecto terminó como un prototipo funcional completo, con menú digital, visor 3D, experiencia de Realidad Aumentada, orden de mesa y confirmación final.

Aunque no está conectado a un sistema real de cocina o pagos, la aplicación demuestra de forma funcional cómo podría implementarse una solución comercial para restaurantes interesados en mejorar la experiencia de sus clientes mediante Realidad Aumentada.

---

## Limitaciones actuales

- No existe integración con sistema real de cocina.
- No procesa pagos reales.
- No cuenta con panel administrativo.
- No tiene analítica real de usuarios.
- La información nutrimental es representativa para el prototipo.
- La experiencia AR depende de la iluminación y visibilidad del marcador.
- El diseño fue optimizado principalmente para tablet Android horizontal.
- El sistema no está conectado a una base de datos externa.

---

## Posibles mejoras futuras

- Integración con sistema real de pedidos.
- Conexión directa con cocina.
- Integración con pagos.
- Panel web para administrar el menú.
- Analítica de platillos más vistos.
- Recomendaciones inteligentes.
- Promociones dinámicas.
- Soporte multi-sucursal.
- Modo mesero.
- Modo cliente con QR.
- Modelos 3D más detallados.
- Soporte multilenguaje.
- Administración de precios en tiempo real.
- Integración con inventario.
- Notificaciones para personal de servicio.

---

## Modelo comercial propuesto

El proyecto puede evolucionar a una solución comercial para restaurantes mediante alguno de los siguientes modelos:

- Pago único por implementación.
- Suscripción mensual por sucursal.
- Modelo híbrido de instalación inicial más mantenimiento.
- Paquetes adicionales para creación de nuevos modelos 3D.
- Servicio de actualización de menú y promociones.

---

## Autor

**Angel Uriel Morales Galicia**  
Proyecto final de Realidad Virtual y Realidad Aumentada  
Facultad de Ingeniería, UNAM  
Semestre 2026-2
