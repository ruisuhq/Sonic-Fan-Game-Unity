# Sonic Fan-Game Unity

Proyecto de videojuego 2D tipo plataforma inspirado en Sonic, desarrollado en Unity. Este repositorio contiene el proyecto editable en `Source/` y un build de demostración en `Sonic.exe`.

## DEMO

<iframe width="1280" height="720" src="https://youtu.be/g7pWSvGMIuM" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>


- Escenas principales: MenuPrincipal, Nivel 1, Nivel 2 y FinJuego.
- Flujo: Menú → Niveles → Fin de juego (con transiciones de fade).
- Audio: música de fondo y pista de jefe con desvanecimiento.

Para probar desde el editor, abre el proyecto Unity en `Source/` y ejecuta la escena `MenuPrincipal`.

## Arquitectura Técnica
- **Motor**: Unity 2022.3.21f1 (ver [Source/ProjectSettings/ProjectVersion.txt](Source/ProjectSettings/ProjectVersion.txt)).
- **Gestión de escenas**: `CargadorEscena` controla el flujo por índices de Build Settings.
	- Ejemplos: `CargarSiguienteNivel(indice=1)`, `CargarMenuPrincipal()`, `ReiniciarNivel()`.
	- Transiciones: Animator con trigger `Empezar` y parámetro `tiempoTransicion`.
- **Estado global**: `ValoresDeJuego` con propiedades estáticas `Vidas` y `Puntaje`.
- **Audio**: `ControladorDeSonidos` como singleton con `DontDestroyOnLoad`.
	- Métodos: `EjecutarSonido(AudioClip)`, `EmpezarMusicaDeFondo()`, `DetenerMusicaDeFondo()`; incluye coroutines de fade-out para música y boss.
- **Enemigos**: scripts bajo `Source/Assets/Scripts/Enemigos` (p.ej., `Cangrejo`, `BombaCangrejo`).
- **Transiciones UI**: Animaciones `FadeIn.anim`, `FadeOut.anim` y `Fade.controller`.
- **Convenciones**: Nombres de scripts y escenas en español; el orden de escenas en Build Settings es crítico (Menu=0, primer nivel=1, último=FinJuego).

## Tecnologías y Paquetes
- Unity 2D Feature Set, TextMeshPro, UGUI, Timeline, Visual Scripting, Cinemachine.
- Ver dependencias en [Source/Packages/manifest.json](Source/Packages/manifest.json).

## Características Principales
- Plataformas y movimiento: `Plataforma_movimiento.cs`, `Puente.cs`.
- Coleccionables y feedback: `RingEstatico.cs`, `RingPerdido.cs`, `RingBrillo.cs`.
- Control del tiempo y estados: `SonicTiempo.cs`, `ControladorSonicEscena.cs`.
- Menús y navegación: `ControladorMenuPrincipal.cs`, `ControladorMenuFinJuego.cs` integrados con `CargadorEscena`.
- Audio: ejecución de SFX y música con transición suave; pista dedicada para boss.

## Estructura del Proyecto
- `Source/Assets/Scenes`: escenas del juego (MenuPrincipal, Nivel 1, Nivel 2, FinJuego, Pruebas).
- `Source/Assets/Scripts`: lógica de juego, UI, audio y enemigos.
- `Source/Assets/Sprites` y `Source/Assets/Sonidos`: assets gráficos y de sonido.
- `Sonic Hypermedia_Data/`: datos del build (no editar).

## Cómo Abrir y Ejecutar
1. Instala Unity 2022.3.21f1.
2. Abre el proyecto en `Source/` o carga la solución [Source/Sonic Hypermedia.sln](Source/Sonic%20Hypermedia.sln) en tu IDE.
3. Verifica el orden de las escenas en Build Settings: MenuPrincipal (0), niveles (1..N), FinJuego (última).
4. Ejecuta `MenuPrincipal` y navega por el juego.

## Pruebas
- Unity Test Framework 1.1.33 disponible.

## Ejemplos de Integración
- Cargar siguiente nivel desde un checkpoint: `FindObjectOfType<CargadorEscena>().CargarSiguienteNivel();`
- Sumar puntos al recoger un ítem: `ValoresDeJuego.Puntaje += 100;` y actualizar UI (TextMeshPro).
- Reproducir sonido de anillo: `ControladorDeSonidos.Instance.EjecutarSonido(ringClip);`.

## Referencias Técnicas
- Unity (LTS 2022.3): https://unity.com/releases/lts
 - Sonic Retro (juego original): https://info.sonicretro.org/Sonic_the_Hedgehog_(16-bit)
 - Sonic Physics Guide: https://info.sonicretro.org/Sonic_Physics_Guide
 - Spriters Resource (sprites): https://www.spriters-resource.com/sega_genesis/sonicth1/

### Tutoriales / Inspiración
- [Plataformas 2D en Unity](https://www.youtube.com/watch?v=u-lMi9akss4)
- [Física y movimiento estilo Sonic](https://www.youtube.com/watch?v=XvmPqkh0BIw)
- [Cinemachine y límites de cámara](https://www.youtube.com/watch?v=KH7qxq-u7gk)
- [Tilemaps y colisiones 2D](https://www.youtube.com/watch?v=8c3S5SJCaRM)
- [UI y HUD con TextMeshPro](https://www.youtube.com/watch?v=OSUVFY_IpEM)
- [Audio: música y SFX con fades](https://www.youtube.com/watch?v=pteErRsKszY)

## Notas y Guardrails
- No edites `Sonic Hypermedia_Data/`; son archivos generados del build.
- Mantén la versión de Unity indicada para evitar re-importaciones masivas.
- Respeta la convención de nombres en español y el orden de escenas.
