# Nombre Que Aún No Tenemos
## Proyecto final Interfaces Inteligentes
### Desarrollado por

- Alejandro Miguel Cruz Quiralte
- Nailea Fayna Cruz Galván
- Sara Darias Sánchez
- Abián Santana Ledesma

# COSAS A HACER (ESTO SE VA A BORRAR)
no olvidar subir apk

para el zip que la profe quiere, solo incluir el readme y scripts (creo, ns si le importa ponerlo todo junto y ya)

hay carpetas creadas pero github las ignora pq no tienen nada, se las digo para q sepan

[media](Media)
[Presentación](Presentación)
[Proyecto_Unity](Proyecto_Unity)
[Scripts](Scripts)

no olvidar comentar el codigo

## Índice

- [Presentación del juego](#presentación-del-juego)
  - [Descripción](#descripción)
  - [Mecánicas](#mecánicas)
  - [Diseño de escenario](#diseño-de-escenario)
  - [Controles](#controles)
  - [GIF demo](#gif-demo)
- [Cuestiones importantes para el uso](#cuestiones-importantes-para-el-uso)
- [Aspectos destacables](#aspectos-destacables)
- [Estructura de los scripts](#estructura-de-los-scripts)
- [Hitos de programación logrados](#hitos-de-programación-logrados)
- [Elementos externos](#elementos-externos)
- [Sensores](#sensores)
- [Acta de acuerdos del grupo](#acta-de-acuerdos-del-grupo)
- [Checklist de recomendaciones de diseño de aplicaciones de RV](#checklist-de-recomendaciones-de-diseño-de-aplicaciones-de-rv)
- [Enlaces de interés](#enlaces-de-interés)
- [Posibles mejoras](#posibles-mejoras)
- [Ejecución del juego (extendido)](#ejecución-del-juego-extendido)


## Presentación del juego

### Descripción
aqui literal describir el juego tipo introducción

### Mecánicas
describirlo más a fondo, con las mecanicas de 

- los demonios atacando a las plantas
- el campesino que nos vende muebles

### Diseño de escenario

En un principio dudamos entre utilizar un mapa ya creado de la Asset Store o fabricarlo nosotros mismos. Tras barajar las opciones, nos decantamos por la segunda. Los mapas que vimos tenían demasiadas inclinaciones y no encajaban bien con la distribución que habíamos planteado. Por ello, aunque supusiera más tiempo, decidimos utilizar el asset [Pandazole - Nature Environment Low poly Pack](https://assetstore.unity.com/packages/3d/environments/pandazole-nature-environment-low-poly-pack-212621), que incluye piezas de tierra y agua que permiten construir el mapa poco a poco. Al habernos dividido el trabajo desde el inicio, pudimos implementarlo sin problemas. Además, nos permitió darle exactamente la forma y los detalles que queríamos, por lo que consideramos que el tiempo invertido mereció la pena.

El gameplay principal se desarrolla en una zona pequeña del mapa, donde se encuentran la casa y el jardín. El resto del mapa es principalmente ambiental y, aunque no tiene una función jugable directa, lo decoramos con árboles, arbustos y otros elementos propios de un bosque para mejorar la inmersión. También incluimos un río y una cueva con fines decorativos, así como un portal que encaja con la historia del juego, ya que es el lugar del que provienen los demonios.

En cuanto al cielo, utilizamos un asset para darle un estilo más visual y de dibujos, perteneciente al paquete [Fantasy Skybox FREE](https://assetstore.unity.com/packages/2d/textures-materials/sky/fantasy-skybox-free-18353).

### Controles
cardboard, mandos de switch.. etc

### GIF demo
un gif del gameplay principal. luego pondremos un gameplay bien

## Cuestiones importantes para el uso
es para android

**Versión de Unity utilizada:** 6000.2.6f2

## Aspectos destacables

## Estructura de los scripts
poner tipo

demonios:
  - este script
  - y este

campesino:
  - ahora este
  - tal

## Hitos de programación logrados

decir que en parte las cosas q hicimos en la asignatura fue la base... como raycast, eventos, delegados, sensores...

Speech Recognizer

efectos de sonido, musica,

uso de sensores

uso de eventos para controlar las mecánicas

menú de compra de muebles

sistema de monedas por matar a los demonios

creación del mapa

utilización de vr

manejo de fisicas usando rigidbody, colliders...

## Elementos externos

- [Pandazole - Nature Environment Low poly Pack](https://assetstore.unity.com/packages/3d/environments/pandazole-nature-environment-low-poly-pack-212621). Asset principal utilizado para el desarrollo del entorno.

- [Level Design Modular Starter Pack](https://assetstore.unity.com/packages/3d/props/level-design-modular-starter-pack-288972). Paquete utilizado para montar la casa del personaje principal.

- [Fantasy Skybox FREE](https://assetstore.unity.com/packages/2d/textures-materials/sky/fantasy-skybox-free-18353). Usado para sustituir el cielo por defecto de Unity por uno con estilo más de dibujos.

- [Modular Medieval Lanterns](https://assetstore.unity.com/packages/3d/environments/historic/modular-medieval-lanterns-85527). Utilizado para las lámparas de la casa.

- [3D Low Poly Magical Forest](https://assetstore.unity.com/packages/3d/environments/fantasy/3d-low-poly-magical-forest-323631). Usado para el portal de los demonios.

- [OpenGameArt](https://opengameart.org/). Página utilizada para la búsqueda de texturas.

## Sensores

giroscopio 100%

## Acta de acuerdos del grupo

Al inicio del proyecto realizamos varias reuniones en Discord para definir los aspectos principales del juego, como el concepto general, las mecánicas principales y el enfoque del desarrollo. Durante estas reuniones debatimos distintas ideas, descartamos algunas y reformulamos otras hasta llegar a una base común clara.

A partir de ahí, establecimos una distribución inicial de tareas que consideramos equitativa para comenzar el desarrollo. Esta organización nos permitió trabajar en paralelo, aunque se fue adaptando con el tiempo según el progreso de cada uno. En todo momento mantuvimos reuniones frecuentes y un contacto constante para compartir avances, resolver dudas y apoyarnos mutuamente cuando alguien terminaba antes su parte.

La distribución principal de tareas fue la siguiente:

- **Alejandro**
  - Diseño e implementación de los enemigos.

- **Nailea**
  - Creación del escenario.

- **Sara**
  - Diseño e implementación de las interacciones con el aldeano y el catálogo de objetos.

- **Abian**
  - Diseño e implementación de la mecánica de las plantas.

Todos participamos en la escritura del README, especialmente en las secciones relacionadas con el trabajo realizado por cada uno.

## Checklist de recomendaciones de diseño de aplicaciones de RV

aqui seria eliminar los q no cumplimos xd

Mantener siempre una velocidad de movimiento constante (sin aceleraciones).

Dejar que el usuario inicie la acción voluntariamente en lugar de que empiece sola.

El usuario debe tener siempre el control del movimiento para anticiparse.

Anclar visualmente al usuario sentado usando referencias como cabinas o sillas.

Mantener el seguimiento de cabeza (Head Tracking) siempre activo para objetos y UI.

Desvanecer la pantalla a negro/color antes de perder el tracking o al cargar escena.

Usar rotaciones de 3 grados de libertad preferiblemente sobre 1 grado.

Evitar cambios bruscos de brillo (de oscuro a claro) para no molestar a los ojos.

La experiencia debe comenzar solo cuando el usuario confirme con un clic.

Colocar los controles de interfaz (UI) dentro del campo de visión inicial.

Actualizar la posición de la UI si el usuario se mueve para que la siga viendo.

Usar retícula solo para objetivos pequeños o al acercarse a ellos.

Separar suficientemente los botones para evitar errores de selección.

Resaltar visualmente (hover/luz) el punto de interacción al mirar.

Usar Audio Espacial para que el sonido localice la posición de los objetos.

Mantener la latencia de respuesta por debajo de los 20 milisegundos.

Evitar colocar objetos con apariencia interactiva que no se puedan coger.

Representar manos estilizadas (cartoon/robot) en lugar de manos humanas realistas.

No representar el cuerpo o brazos del usuario (solo las manos).

Cuidar la escala de los objetos para que coincida con la realidad.

Evitar obligar al usuario a mirar más de 60º hacia arriba.

Evitar obligar al usuario a mirar más de 40º hacia abajo.

Colocar el contenido cómodo entre los 0.5m y los 20m de distancia.

Evitar acciones físicas peligrosas en el mundo real (caminar mucho, agacharse rápido).

Marcar visualmente el área de juego si se requiere movimiento real.

## Enlaces de interés

## Posibles mejoras
nos faltó tiempo, queríamos implementar taltal

poner mas plantas

incluir un modo historia

aumentar el tamaño del mapa

## Ejecución del juego (extendido)
aquí iría un gameplay to wapo subido a youtube y narrado por el mismísimo vegetta777