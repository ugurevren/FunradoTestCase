using System.Collections;
using UnityEngine;

namespace Character
{
    public class EnemyController : Character
    {
        public float speed = 5;
        public float waitTime = .3f;
        public float turnSpeed = 90;
        private bool _isMoving;
        
        private Animator _animator;
        
        public Transform pathHolder;
        
        public Light spotlight;
        public float viewDistance;
        public LayerMask viewMask;
        private float _viewAngle;
        
        private Transform _playerTransform;
        private Color _originalSpotlightColor;

        private void Awake()
        {
            _floatingTextObject = GetComponentInChildren<FloatingText>().gameObject;
            
        }

        private void Start()
        {
            Debug.Log("çalıştı");
            _animator = GetComponent<Animator>();
            
            _playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
            _floatingText= _floatingTextObject.GetComponent<FloatingText>();
            UpdateLevelText(level);
            
            
            _viewAngle  = spotlight.spotAngle;
            _originalSpotlightColor = spotlight.color;

            Vector3[] waypoints = new Vector3[pathHolder.childCount];
            for (int i = 0; i < waypoints.Length; i++) {
                waypoints [i] = pathHolder.GetChild (i).position;
                waypoints [i] = new Vector3 (waypoints [i].x, transform.position.y, waypoints [i].z);
            }

            StartCoroutine (FollowPath (waypoints));
        }
        private void Update()
        {
            _animator.SetBool("isMoving", _isMoving);
            if (CanSeePlayer ()) {
                
                spotlight.color = Color.red;
            } else {
                spotlight.color = _originalSpotlightColor;
            }
        }
        bool CanSeePlayer() {
            if (Vector3.Distance(transform.position,_playerTransform.position) < viewDistance) {
                Vector3 dirToPlayer = (_playerTransform.position - transform.position).normalized;
                float angleBetweenGuardAndPlayer = Vector3.Angle (transform.forward, dirToPlayer);
                if (angleBetweenGuardAndPlayer < _viewAngle / 2f) {
                    if (!Physics.Linecast (transform.position, _playerTransform.position, viewMask)) {
                        return true;
                    }
                }
            }
            return false;
        }
        private IEnumerator FollowPath(Vector3[] waypoints) {
            transform.position = waypoints [0];

            int targetWaypointIndex = 1;
            Vector3 targetWaypoint = waypoints [targetWaypointIndex];
            transform.LookAt (targetWaypoint);
            
            while (true) {
                transform.position = Vector3.MoveTowards (transform.position, targetWaypoint, speed * Time.deltaTime);
                _isMoving = true;
                
                if (transform.position == targetWaypoint) {
                    _isMoving = false;
                    targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                    targetWaypoint = waypoints [targetWaypointIndex];
                    yield return new WaitForSeconds (waitTime);
                    yield return StartCoroutine (TurnToFace (targetWaypoint));
                }
                yield return null;
            }
        }

        private IEnumerator TurnToFace(Vector3 lookTarget) {
            Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
            float targetAngle = 90 - Mathf.Atan2 (dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

            while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f) {
                float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.up * angle;
                yield return null;
            }
        }

        private void OnDrawGizmos() {
            Vector3 startPosition = pathHolder.GetChild (0).position;
            Vector3 previousPosition = startPosition;

            foreach (Transform waypoint in pathHolder) {
                Gizmos.DrawSphere (waypoint.position, .3f);
                Gizmos.DrawLine (previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine (previousPosition, startPosition);

            Gizmos.color = Color.red;
            Gizmos.DrawRay (transform.position, transform.forward * viewDistance);
        }

        public override void Combat(Collider other)
        {
            int playerLevel = other.GetComponent<Character>().level;

            if (playerLevel > level)
            {
                gameObject.GetComponent<Animator>().enabled = false;
                // beklet ve sil
                Destroy(gameObject, 1f);
            }
        }
    }
}
