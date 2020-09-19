//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class Enemy_Old : MonoBehaviour
//{
//    public enum MovementBehaviour
//    {
//        None,
//        Seek,
//        Flee,
//        Pursuit,
//        Evade
//    }
//    public MovementBehaviour movementBehaviour;
//    public bool avoidObstacles;
//    public float collisionAvoidDst = 3f;
//    public float avoidCollisionWeight = 1f;
//    public BoidSettings settings;
//    public LayerMask layerMask;

//    public float mass = 1f;
//    public float maxSpeed = 2f;
//    public float maxForce = 2f;
//    public float predictionMultiplier = 1f;
//    public float maxSteerForce = 1f;

//    [Space]
//    public float timeBeforeDestroyAfterDeath = 2f;

//    public Damageable Damageable { get; private set; }
//    public EnemySpawner EnemySpawner { get; set; }

//    private Transform _target;
//    public Transform Target
//    {
//        get
//        {
//            return _target;
//        }
//        set
//        {
//            _target = value;

//            _targetRigidBody = _target.GetComponent<Rigidbody>();
//        }
//    }
//    private Vector3 _velocity;
//    private Vector3 _steeringDirection;
//    private Vector3 _acceleration;
//    private Vector3 _approximateUp;
//    private Rigidbody _targetRigidBody;
//    private Collider _collider;

//    private void Start()
//    {
//        Target = FindObjectOfType<Player>().transform;
//        Damageable = GetComponent<Damageable>();
//        _collider = GetComponentInChildren<Collider>();

//        Damageable.Death += Damageable_Death;
//    }

//    private void Damageable_Death()
//    {
//        _collider.enabled = false;

//        EnemySpawner.RemoveEnemy(this);

//        Destroy(gameObject, timeBeforeDestroyAfterDeath);
//    }

//    private void Update()
//    {
//        Vector3 acceleration = Vector3.zero;

//        if (movementBehaviour == MovementBehaviour.None)
//        {
//            return;
//        }
//        if (Target != null)
//        {
//            Vector3 offsetToTarget = (Target.position - transform.position);
//            acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
//        }




//        if (movementBehaviour == MovementBehaviour.Seek)
//        {
//            Vector3 desiredVelocity = Vector3.Normalize(Target.position - transform.position) * maxSpeed;
//            _steeringDirection = desiredVelocity - _velocity;
//        }
//        else if (movementBehaviour == MovementBehaviour.Flee)
//        {
//            Vector3 desiredVelocity = Vector3.Normalize(Target.position - transform.position) * maxSpeed;
//            _steeringDirection = -desiredVelocity - _velocity;
//        }
//        else if (movementBehaviour == MovementBehaviour.Pursuit)
//        {
//            if (_targetRigidBody != null)
//            {
//                Vector3 distance = Target.position - transform.position;
//                float predictionAmount = distance.magnitude * predictionMultiplier;
//                Vector3 predictedPosition = Target.position + _targetRigidBody.velocity * predictionAmount * Time.deltaTime;
//                Vector3 desiredVelocity = Vector3.Normalize(predictedPosition - transform.position) * maxSpeed;

//                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//                sphere.GetComponent<Collider>().enabled = false;
//                sphere.transform.position = predictedPosition;
//                Destroy(sphere, 0.1f);

//                _steeringDirection = desiredVelocity - _velocity;
//            }
//        }
//        else if (movementBehaviour == MovementBehaviour.Evade)
//        {
//            if (_targetRigidBody != null)
//            {
//                Vector3 distance = Target.position - transform.position;
//                float predictionAmount = distance.magnitude * predictionMultiplier;
//                Vector3 predictedPosition = Target.position + _targetRigidBody.velocity * predictionAmount;
//                Vector3 desiredVelocity = Vector3.Normalize(predictedPosition - transform.position) * maxSpeed;

//                //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//                //sphere.transform.position = predictedPosition;
//                //Destroy(sphere, 0.1f);

//                _steeringDirection = -desiredVelocity - _velocity;
//            }
//        }





//        //if (avoidObstacles && IsHeadingForCollision(steeringForce))
//        //{
//        //    Vector3 collisionAvoidDir = ObstacleRays(steeringForce);
//        //    Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * avoidCollisionWeight;
//        //    _acceleration += collisionAvoidForce;

//        //    //_steeringDirection = FindUnobstructedDirection(transform.forward);
//        //}

//        Vector3 steeringForce = Vector3.ClampMagnitude(_steeringDirection, maxForce);
//        _acceleration = steeringForce / mass;
//        _velocity = Vector3.ClampMagnitude(_velocity + _acceleration, maxSpeed);

//        transform.position = transform.position + _velocity;


//        //LOOK
//        Vector3 newForward = Vector3.Normalize(_velocity);
//        _approximateUp = Vector3.Normalize(_approximateUp);
//        Vector3 newSide = Vector3.Cross(newForward, _approximateUp);
//        Vector3 newUp = Vector3.Cross(newForward, newSide);
//        Quaternion rotation = Quaternion.LookRotation(newForward, newUp);
//        transform.rotation = rotation;

//        //Vector3 lookTargetDir = _target.position - transform.position;
//        ////lookTargetDir.y = 0.0f;
//        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookTargetDir), Time.time * lookAtSpeed);




//        //var currentPosition = transform.position;
//        //var currentRotation = transform.rotation;

//        //// Current velocity randomized with noise.
//        //var noise = Mathf.PerlinNoise(Time.time, _noiseOffset) * 2.0f - 1.0f;
//        //var velocity = baseVelocity * (1.0f + noise * velocityVariation);

//        //// Initializes the vectors.
//        //var separation = Vector3.zero;
//        //var alignment = Vector3.forward;
//        //var cohesion = Vector3.zero;

//        //// Looks up nearby boids.
//        //var nearbyBoids = Physics.OverlapSphere(currentPosition, neighborDist, layerMask);

//        //// Accumulates the vectors.
//        //foreach (var boid in nearbyBoids)
//        //{
//        //    if (boid.gameObject == gameObject) 
//        //        continue;

//        //    var t = boid.transform;
//        //    separation += GetSeparationVector(t);
//        //    alignment += t.forward;
//        //    cohesion += t.position;
//        //}

//        //var avg = 1.0f / nearbyBoids.Length;
//        //alignment *= avg;
//        //cohesion *= avg;
//        //cohesion = (cohesion - currentPosition).normalized;

//        //// Calculates a rotation from the vectors.
//        //var direction = separation + alignment + cohesion;
//        //var rotation = Quaternion.FromToRotation(Vector3.forward, direction.normalized);

//        //// Applys the rotation with interpolation.
//        //if (rotation != currentRotation)
//        //{
//        //    var ip = Mathf.Exp(rotationCoeff * Time.deltaTime);
//        //    transform.rotation = Quaternion.Slerp(rotation, currentRotation, ip);
//        //}

//        //// Moves forawrd.
//        //transform.position = currentPosition + transform.forward * (velocity * Time.deltaTime);
//    }

//    private Vector3 SteerTowards(Vector3 vector)
//    {
//        Vector3 v = vector.normalized * maxSpeed - _velocity;
//        return Vector3.ClampMagnitude(v, maxSteerForce);
//    }

//    private bool IsHeadingForCollision(Vector3 forward)
//    {
//        RaycastHit hit;
//        if (Physics.Raycast(transform.position, forward, out hit, collisionAvoidDst, layerMask))
//        {
//            return true;
//        }
//        else { }
//        return false;
//    }

//    private Vector3 ObstacleRays(Vector3 forward)
//    {
//        Vector3[] rayDirections = EnemyHelper.directions;

//        for (int i = 0; i < rayDirections.Length; i++)
//        {
//            Vector3 dir = transform.TransformDirection(rayDirections[i]);
//            Ray ray = new Ray(transform.position, dir);
//            if (!Physics.Raycast(ray, collisionAvoidDst, layerMask))
//            {
//                return dir;
//            }
//        }

//        return forward;
//    }

//    private Vector3 FindUnobstructedDirection(Vector3 steeringDirection)
//    {
//        Vector3 bestDir = steeringDirection;
//        float furthestUnobstructedDistance = 0f;

//        RaycastHit hit;
//        for (int i = 0; i < EnemyHelper.directions.Length; i++)
//        {
//            // transform ray from local to world space so that smaller dir changes are examined first
//            Vector3 dir = transform.TransformDirection(EnemyHelper.directions[i]);

//            if (Physics.Raycast/*SphereCast*/(transform.position, dir,  out hit, 2f, layerMask))
//            {
//                float distance = hit.distance;
//                if (distance > furthestUnobstructedDistance)
//                {
//                    furthestUnobstructedDistance = distance;
//                }
//            }
//            else
//            {
//                return dir;
//            }
//        }

//        return bestDir;
//    }

//    // Calculates the separation vector with a target.
//    //Vector3 GetSeparationVector(Transform target)
//    //{
//    //    var diff = transform.position - target.transform.position;
//    //    var diffLen = diff.magnitude;
//    //    var scaler = Mathf.Clamp01(1.0f - diffLen / neighborDist);
//    //    return diff * (scaler / diffLen);
//    //}
//}

//public static class EnemyHelper
//{
//    const int numViewDirections = 300;
//    public static readonly Vector3[] directions;

//    static EnemyHelper()
//    {
//        directions = new Vector3[EnemyHelper.numViewDirections];

//        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
//        float angleIncrement = Mathf.PI * 2 * goldenRatio;

//        for (int i = 0; i < numViewDirections; i++)
//        {
//            float t = (float)i / numViewDirections;
//            float inclination = Mathf.Acos(1 - 2 * t);
//            float azimuth = angleIncrement * i;

//            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
//            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
//            float z = Mathf.Cos(inclination);
//            directions[i] = new Vector3(x, y, z);
//        }
//    }
//}