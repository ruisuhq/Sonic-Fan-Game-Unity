# Documentación Técnica para Desarrolladores

Este documento resume los aspectos técnicos necesarios para desarrollar y mantener el proyecto Sonic Fan-Game Unity.

## Entorno y Versionado
- Unity: 2022.3.21f1 (ver [Source/ProjectSettings/ProjectVersion.txt](../Source/ProjectSettings/ProjectVersion.txt#L1-L2)). Mantener esta versión para evitar reimportaciones.
- Solución: abrir [Source/Sonic Hypermedia.sln](../Source/Sonic%20Hypermedia.sln) o el proyecto en `Source/` con Unity Hub.
- Paquetes: revisar y gestionar en [Source/Packages/manifest.json](../Source/Packages/manifest.json).

## Estructura de Carpetas
- `Source/Assets/Scenes`: escenas de juego (MenuPrincipal, Nivel 1, Nivel 2, FinJuego, Pruebas).
- `Source/Assets/Scripts`: lógica de juego, UI, audio y enemigos.
- `Source/Assets/Sprites`, `Source/Assets/Sonidos`: assets gráficos y de audio.
- `Sonic Hypermedia_Data/`: salida de build (no editar).

## Gestión de Escenas y Transiciones
- Control central en [Source/Assets/Scripts/CargadorEscena.cs](../Source/Assets/Scripts/CargadorEscena.cs#L1-L39):
	- `CargarSiguienteNivel(int indice = 1)`: avanza o retrocede según índice.
	- `CargarMenuPrincipal()`: índice 0.
	- `CargarMenuFinJuego()`: última escena en Build Settings.
	- `ReiniciarNivel()` y `ReiniciarJuego()`.
- Transiciones: usar Animator con trigger `Empezar` y respetar `tiempoTransicion`.
- Convención de orden: MenuPrincipal (0), niveles (1..N), FinJuego (última). Verifica Build Settings al añadir escenas.

## Estado Global y HUD
- [Source/Assets/Scripts/ValoresDeJuego.cs](../Source/Assets/Scripts/ValoresDeJuego.cs#L1-L12) define `Vidas` y `Puntaje` (estáticos).
- HUD de tiempo: [Source/Assets/Scripts/SonicTiempo.cs](../Source/Assets/Scripts/SonicTiempo.cs#L1-L65) usa `TMP_Text` para mostrar `mm:ss`, controla pausa/reanudación y activa el canvas `TiempoAcabado` si se alcanza el límite.
- Puntaje/Vidas: se almacenan en `ValoresDeJuego`; para mostrarlos, vincula TextMeshProUGUI por Inspector y actualiza el texto leyendo `ValoresDeJuego` en `Update()` o al producirse eventos (recolección de anillos, muerte, checkpoints).

## Audio
- Singleton en [Source/Assets/Scripts/ControladorDeSonidos.cs](../Source/Assets/Scripts/ControladorDeSonidos.cs#L1-L94):
	- `Instance` con `DontDestroyOnLoad`.
	- SFX: `EjecutarSonido(AudioClip)`.
	- Música: `EmpezarMusicaDeFondo()`, `DetenerMusicaDeFondo()` con `FadeOutMusica()`.
	- Boss: `EmpezarBossDeFondo()`, `DetenerBossDeFondo()` con `FadeOutBoss()`.
- Recomendación: referenciar `ControladorDeSonidos.Instance` desde scripts de gameplay para SFX contextuales.

## Enemigos y Gameplay
- Carpeta: [Source/Assets/Scripts/Enemigos](../Source/Assets/Scripts/Enemigos).
- Ejemplos:
	- `Cangrejo.cs`: comportamiento de enemigo estándar.
	- `BombaCangrejo.cs`: lógica de proyectil/ataque.
- Otros componentes:
	- `Plataforma_movimiento.cs`, `Puente.cs`: movimiento/colisiones de plataformas.
	- `RingEstatico.cs`, `RingPerdido.cs`, `RingBrillo.cs`: coleccionables y feedback visual.
	- `SonicTiempo.cs`, `ControladorSonicEscena.cs`: gestión de tiempo y control de estado de escena.

## Flujo de Menús
- `ControladorMenuPrincipal.cs` y `ControladorMenuFinJuego.cs`: disparan métodos de `CargadorEscena` para navegar.
- Asegura que los botones UI llamen a estos controladores y que el Animator de transición esté referenciado en la escena.

## Flujo de Boss/Eventos
- Script principal: [Source/Assets/Scripts/BossEvent.cs](../Source/Assets/Scripts/BossEvent.cs#L1-L74).
- Inicio (`Start`): desactiva `Barrera[]` y establece el confiner de cámara (`CinemachineConfiner.m_BoundingShape2D`) a `Limites[0]` (modo normal). Obtiene `Eggman_Controlador_nuevo` desde `Eggman`.
- Activación (`IniciarBossEvent`):
	- Deshabilita `InicioBoss` (un `EdgeCollider2D` usado como zona de disparo).
	- Activa `Barrera[]` (cierra la arena) y cambia el confiner a `Limites[1]`.
	- Cambia la música: `DetenerMusicaDeFondo()` → `EmpezarBossDeFondo()`.
	- Cambia el estado del jefe: `scriptEggman.Estado = 1`.
- Finalización (`FinalizarBoss`): desactiva barreras, restaura música (`DetenerBossDeFondo()` → `EmpezarMusicaDeFondo()`) y recupera confiner a `Limites[0]`.
- Configuración en escena (Inspector): asignar `InicioBoss`, `Eggman`, los dos objetos de `Barrera[]`, `LimiteCamaraBoss` y `Limites[0..1]`. Invocar `IniciarBossEvent()` al entrar el jugador en `InicioBoss` (p. ej., vía `OnTriggerEnter2D`).

## Workflows de Desarrollo
- Añadir una escena de nivel:
	1. Crear la escena en `Source/Assets/Scenes`.
	2. Agregar a Build Settings respetando el orden.
	3. Verificar referencias del `CargadorEscena` y Animator de transición.
- Incorporar nuevos assets:
	- Importar por Unity para generar `.meta`.
	- Colocar sprites en `Sprites/` y sonidos en `Sonidos/`.
- Sonido de un ítem:
	- En el script del ítem, invocar `ControladorDeSonidos.Instance.EjecutarSonido(clip);`.

## Builds y Plataformas
- Orden de escenas: MenuPrincipal (0) → niveles (1..N) → FinJuego (última).
- Plataformas objetivo (Editor): Windows (x86_64), macOS, Linux (x86_64). Selecciona según necesidad.
- Player Settings recomendadas:
	- Resolución/aspecto 16:9 (p. ej., 1920×1080) para encajar con assets 2D.
	- Icono del juego: `Source/Assets/icon.png`.
	- Verifica VSync/calidad según rendimiento requerido.
- Pasos de build (GUI):
	1. `File → Build Settings…`.
	2. Añadir escenas en orden si falta alguna.
	3. Elegir plataforma destino y pulsar `Switch Platform` si aplica.
	4. Ajustar `Player Settings…` (resolución, iconos, empresa/producto).
	5. `Build` y seleccionar carpeta de salida.
- Notas: no edites manualmente `Sonic Hypermedia_Data/`; es salida del build. Si cambias de versión de Unity, valida paquetes y reimportación.

## Pruebas
- Framework: Unity Test Framework 1.1.33.
- Ubicación sugerida: `Source/Assets/Tests` (EditMode/PlayMode).
- Buenas prácticas del proyecto: tests ligeros para lógica pura (no UI), y pruebas de integración mínimas para gestión de escenas.

## Guías de Estilo y Convenciones
- Nombres y comentarios en español.
- Evitar reestructurar ampliamente: cambios pequeños y enfocados.
- Mantener APIs públicas y patrones ya usados (MonoBehaviour, métodos y nombres existentes).

## Referencias
- Unity LTS 2022.3: https://unity.com/releases/lts
- Cinemachine: https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/index.html
- TextMeshPro: https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/manual/index.html
- Timeline: https://docs.unity3d.com/Packages/com.unity.timeline@1.7/manual/index.html
- Unity Test Framework: https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/index.html
