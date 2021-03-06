using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using Microsoft.DirectX;
using TgcViewer;

namespace Examples.Collision.SphereTriangleCollision
{
    /// <summary>
    /// Herramienta para realizar el movimiento de una Esfera con detección de colisiones,
    /// efecto de Sliding y gravedad.
    /// Basado en el paper de Kasper Fauerby
    /// http://www.peroxide.dk/papers/collision/collision.pdf
    /// Su utiliza una estrategia distinta al paper en el nivel más bajo de colisión.
    /// No se analizan colisiones a nivel de tríangulo, sino que todo objeto se descompone
    /// a nivel de un BoundingBox con 6 caras rectangulares.
    /// 
    /// </summary>
    public class SphereTriangleCollisionManager
    {
        const float EPSILON = 0.05f;
        static readonly Vector3 UP_VECTOR = new Vector3(0, 1, 0);
        

        private Vector3 gravityForce;
        /// <summary>
        /// Vector que representa la fuerza de gravedad.
        /// Debe tener un valor negativo en Y para que la fuerza atraiga hacia el suelo
        /// </summary>
        public Vector3 GravityForce
        {
            get { return gravityForce; }
            set { gravityForce = value; }
        }

        private bool gravityEnabled;
        /// <summary>
        /// Habilita o deshabilita la aplicación de fuerza de gravedad
        /// </summary>
        public bool GravityEnabled
        {
            get { return gravityEnabled; }
            set { gravityEnabled = value; }
        }

        private float slideFactor;
        /// <summary>
        /// Multiplicador de la fuerza de Sliding
        /// </summary>
        public float SlideFactor
        {
            get { return slideFactor; }
            set { slideFactor = value; }
        }

        Vector3 lastCollisionNormal;
        /// <summary>
        /// Normal de la ultima superficie con la que hubo colision
        /// </summary>
        public Vector3 LastCollisionNormal
        {
            get { return lastCollisionNormal; }
        }

        private float onGroundMinDotValue;
        /// <summary>
        /// Valor que indica la maxima pendiente que se puede trepar sin empezar
        /// a sufrir los efectos de gravedad. Valor entre [0, 1] siendo 0 que puede
        /// trepar todo y 1 que no puede trepar nada.
        /// El valor Y de la normal de la superficie contra la que se colisiona tiene
        /// que ser superior a este parametro para permitir trepar la pendiente.
        /// </summary>
        public float OnGroundMinDotValue
        {
            get { return onGroundMinDotValue; }
            set { onGroundMinDotValue = value; }
        }

        Vector3 lastCollisionPoint;
        /// <summary>
        /// Ultimo punto de colision
        /// </summary>
        public Vector3 LastCollisionPoint
        {
            get { return lastCollisionPoint; }
        }

        bool collision;
        /// <summary>
        /// Indica si hubo colision
        /// </summary>
        public bool Collision
        {
            get { return collision; }
        }

        Vector3 lastMovementVector;
        /// <summary>
        /// Ultimo vector de desplazamiento real.
        /// Indica lo que realmente se pudo mover.
        /// </summary>
        public Vector3 LastMovementVector
        {
            get { return lastMovementVector; }
        }

        List<Collider> objetosCandidatos;
        TgcBoundingSphere movementSphere;
        Collider lastCollider;
       


        public SphereTriangleCollisionManager()
        {
            gravityEnabled = true;
            gravityForce = new Vector3(0, -10, 0);
            slideFactor = 1.3f;
            lastCollisionNormal = Vector3.Empty;
            movementSphere = new TgcBoundingSphere();
            objetosCandidatos = new List<Collider>();
            lastCollider = null;
            onGroundMinDotValue = 0.8f;
            collision = false;
            lastMovementVector = Vector3.Empty;
        }

        /// <summary>
        /// Mover BoundingSphere con detección de colisiones, sliding y gravedad.
        /// Se actualiza la posición del centrodel BoundingSphere.
        /// </summary>
        /// <param name="characterSphere">BoundingSphere del cuerpo a mover</param>
        /// <param name="movementVector">Movimiento a realizar</param>
        /// <param name="colliders">Obstáculos contra los cuales se puede colisionar</param>
        /// <returns>Desplazamiento relativo final efecutado al BoundingSphere</returns> 
        public Vector3 moveCharacter(TgcBoundingSphere characterSphere, Vector3 movementVector, List<Collider> colliders)
        {
            Vector3 originalSphereCenter = characterSphere.Center;


            //Mover
            collideWithWorld(characterSphere, movementVector, colliders, true, 1);

            //Aplicar gravedad
            if (gravityEnabled)
            {
                collideWithWorld(characterSphere, gravityForce, colliders, true, onGroundMinDotValue);
            }

            //Calcular el desplazamiento real que hubo
            lastMovementVector = characterSphere.Center - originalSphereCenter;
            return lastMovementVector;
        }

        /// <summary>
        /// Detección de colisiones, filtrando los obstaculos que se encuentran dentro del radio de movimiento
        /// </summary>
        private void collideWithWorld(TgcBoundingSphere characterSphere, Vector3 movementVector, List<Collider> colliders, bool sliding, float slidingMinY)
        {
            //Ver si la distancia a recorrer es para tener en cuenta
            float distanceToTravelSq = movementVector.LengthSq();
            if (distanceToTravelSq < EPSILON)
            {
                return;
            }

            //Dejar solo los obstáculos que están dentro del radio de movimiento de la esfera
            Vector3 halfMovementVec = Vector3.Multiply(movementVector, 0.5f);
            movementSphere.setValues(
                characterSphere.Center + halfMovementVec,
                halfMovementVec.Length() + characterSphere.Radius
                );
            objetosCandidatos.Clear();
            foreach (Collider collider in colliders)
            {
                if (collider.Enable && TgcCollisionUtils.testSphereSphere(movementSphere, collider.BoundingSphere))
                {
                    objetosCandidatos.Add(collider);
                }
            }

            //Detectar colisiones y deplazar con sliding
            doCollideWithWorld(characterSphere, movementVector, objetosCandidatos, 0, movementSphere, sliding, slidingMinY);
        }



        /// <summary>
        /// Detección de colisiones recursiva
        /// </summary>
        public void doCollideWithWorld(TgcBoundingSphere characterSphere, Vector3 movementVector, List<Collider> colliders, int recursionDepth, TgcBoundingSphere movementSphere, bool sliding, float slidingMinY)
        {
            //Limitar recursividad
            if (recursionDepth > 5)
            {
                return;
            }

            //Posicion deseada
            Vector3 originalSphereCenter = characterSphere.Center;
            Vector3 nextSphereCenter = originalSphereCenter + movementVector;

            //Buscar el punto de colision mas cercano de todos los objetos candidatos
            collision = false;
            Vector3 q;
            float t;
            Vector3 n;
            float minT = float.MaxValue;
            foreach (Collider collider in colliders)
            {
                //Colisionar Sphere en movimiento contra Collider (cada Collider resuelve la colision)
                if (collider.intersectMovingSphere(characterSphere, movementVector, movementSphere, out t, out q, out n))
                {
                    //Quedarse con el menor instante de colision
                    if(t < minT)
                    {
                        minT = t;
                        collision = true;
                        lastCollisionPoint = q;
                        lastCollisionNormal = n;
                        lastCollider = collider;
                    }
                }
            }

            //Si nunca hubo colisión, avanzar todo lo requerido
            if (!collision)
            {
                //Avanzar todo lo pedido
                //lastCollisionDistance = movementVector.Length();
                characterSphere.moveCenter(movementVector);
                return;
            }


            //Solo movernos si ya no estamos muy cerca
            if (minT >= EPSILON)
            {
                //Restar un poco al instante de colision, para movernos hasta casi esa distancia
                minT -= EPSILON;
                Vector3 realMovementVector = movementVector * minT;

                //Mover el BoundingSphere
                characterSphere.moveCenter(realMovementVector);

                //Quitarle al punto de colision el EPSILON restado al movimiento, para no afectar al plano de sliding
                Vector3 v = Vector3.Normalize(realMovementVector);
                lastCollisionPoint -= v * EPSILON;
            }


            if (sliding)
            {
                //Calcular plano de Sliding, como un plano tangete al punto de colision con la esfera, apuntando hacia el centro de la esfera
                Vector3 slidePlaneOrigin = lastCollisionPoint;
                Vector3 slidePlaneNormal = characterSphere.Center - lastCollisionPoint;
                slidePlaneNormal.Normalize();
                Plane slidePlane = Plane.FromPointNormal(slidePlaneOrigin, slidePlaneNormal);


                //Calcular vector de movimiento para sliding, proyectando el punto de destino original sobre el plano de sliding
                float distance = TgcCollisionUtils.distPointPlane(nextSphereCenter, slidePlane);
                Vector3 newDestinationPoint = nextSphereCenter - distance * slidePlaneNormal;
                Vector3 slideMovementVector = newDestinationPoint - lastCollisionPoint;

                //No hacer recursividad si es muy pequeño
                slideMovementVector.Scale(slideFactor);
                if (slideMovementVector.Length() < EPSILON)
                {
                    return;
                }

                if (lastCollisionNormal.Y <= slidingMinY)
                {
                    //Recursividad para aplicar sliding
                    doCollideWithWorld(characterSphere, slideMovementVector, colliders, recursionDepth + 1, movementSphere, sliding, slidingMinY);
                }


                
            }
        }

        /// <summary>
        /// Indica si el objeto se encuentra con los pies sobre alguna superficie, sino significa
        /// que está cayendo o saltando.
        /// </summary>
        /// <returns>True si el objeto se encuentra parado sobre una superficie</returns>
        public bool isOnTheGround()
        {
            if(lastCollisionNormal == Vector3.Empty)
                return false;

            //return true;
            //return lastCollisionNormal.Y >= onGroundMinDotValue;
            return lastCollisionNormal.Y >= 0;
        }
        

    }
}
